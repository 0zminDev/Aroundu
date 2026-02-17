using Microsoft.Data.SqlClient;
using Respawn;

namespace Aroundu.IntegrationTests.Respawn
{
    public static class IntegrationTestDbCleanup
    {
        private static readonly Dictionary<string, Respawner> respawners = new();

        public static async Task ResetAsync(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                TrustServerCertificate = true,
                ConnectTimeout = 60,
                ConnectRetryCount = 5,
                ConnectRetryInterval = 2,
                MultiSubnetFailover = true
            };

            var finalConnectionString = builder.ConnectionString;

            if (!respawners.ContainsKey(finalConnectionString))
            {
                await ExecuteWithRetry(async () =>
                {
                    await using var conn = new SqlConnection(finalConnectionString);
                    await conn.OpenAsync();
                    respawners[finalConnectionString] = await Respawner.CreateAsync(conn, new RespawnerOptions
                    {
                        TablesToIgnore = ["__EFMigrationsHistory"],
                        DbAdapter = DbAdapter.SqlServer
                    });
                });
            }

            await using var connection = new SqlConnection(finalConnectionString);
            await connection.OpenAsync();
            await respawners[finalConnectionString].ResetAsync(connection);
        }

        private static async Task ExecuteWithRetry(Func<Task> action)
        {
            var retryCount = 0;
            while (true)
            {
                try
                {
                    await action();
                    break;
                }
                catch (SqlException) when (retryCount < 3)
                {
                    retryCount++;
                    await Task.Delay(2000);
                }
            }
        }
    }
}
