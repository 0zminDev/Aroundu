using Aspire.Hosting;

namespace Aroundu.IntegrationTests.Config.Extenstions
{
    public static class AspireExtensions
    {
        /// <summary>
        /// Waits for the specified resources to reach the "Running" state, instead of the Helathy state thats default in Aspire.
        /// </summary>
        public static async Task WaitForResourcesAsync(
            this DistributedApplication app,
            IEnumerable<string> resourceNames,
            CancellationToken ct = default
        )
        {
            var notificationService =
                app.Services.GetRequiredService<ResourceNotificationService>();

            await Task.WhenAll(
                resourceNames.Select(name =>
                    notificationService.WaitForResourceAsync(name, "Running", ct)
                )
            );
        }

        public static Task WaitForSystemReadyAsync(
            this DistributedApplication app,
            CancellationToken ct = default
        )
        {
            return app.WaitForResourcesAsync(new[] { "sql-server", "messaging" }, ct);
        }
    }
}
