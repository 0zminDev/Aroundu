using Aroundu.Auth.Service.Infrastructure.EFCore;
using Aroundu.Events.Service.Infrastructure.EFCore;
using Aroundu.IntegrationTests.Config.Extenstions;
using Aspire.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace Aroundu.IntegrationTests
{
    public class ApplicationTestingFactory : IAsyncLifetime
    {
        public DistributedApplication App { get; private set; } = default!;
        public HttpClient GatewayClient { get; private set; } = default!;

        public async ValueTask InitializeAsync()
        {
            var appHost =
                await DistributedApplicationTestingBuilder.CreateAsync<Projects.Aroundu_AppHost>();

            appHost.Environment.EnvironmentName = "Testing";

            App = await appHost.BuildAsync();
            await App.StartAsync();

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await App.WaitForSystemReadyAsync(cts.Token);

            await EnsureDatabasesMigratedAsync();

            GatewayClient = App.CreateHttpClient("aroundu-api-gateway");
        }

        private async Task EnsureDatabasesMigratedAsync()
        {
            var eventsCs = await GetConnectionString("events-db");
            var authCs = await GetConnectionString("auth-db");

            await MigrateDatabase<EventsDbContext>(eventsCs);
            await MigrateDatabase<AuthDbContext>(authCs);
        }

        private async Task MigrateDatabase<TContext>(string connectionString)
            where TContext : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }
            );

            using var context = (TContext)
                Activator.CreateInstance(typeof(TContext), optionsBuilder.Options)!;
            var pipeline = CreateRetryPipeline();
            await pipeline.ExecuteAsync(
                async (ct) =>
                {
                    await context.Database.MigrateAsync(ct);
                }
            );
        }

        public async Task<string> GetConnectionString(string dbResourceName) =>
            await App.GetConnectionStringAsync(dbResourceName) ?? "";

        private ResiliencePipeline CreateRetryPipeline()
        {
            return new ResiliencePipelineBuilder()
                .AddRetry(
                    new RetryStrategyOptions
                    {
                        ShouldHandle = new PredicateBuilder()
                            .Handle<SqlException>()
                            .Handle<InvalidOperationException>(),
                        BackoffType = DelayBackoffType.Exponential,
                        UseJitter = true,
                        MaxRetryAttempts = 5,
                        Delay = TimeSpan.FromSeconds(2),
                    }
                )
                .Build();
        }

        public async ValueTask DisposeAsync()
        {
            if (App is not null)
            {
                GatewayClient?.Dispose();

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                try
                {
                    await App.StopAsync(cts.Token);
                }
                catch (Exception) { }
                finally
                {
                    await Task.Run(async () =>
                        {
                            try
                            {
                                await App.DisposeAsync();
                            }
                            catch { }
                        })
                        .WaitAsync(TimeSpan.FromSeconds(5));
                }
            }
        }
    }
}
