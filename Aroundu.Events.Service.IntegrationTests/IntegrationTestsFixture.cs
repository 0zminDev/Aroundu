using Aspire.Hosting;

namespace Aroundu.Events.Service.IntegrationTests
{
    public class IntegrationTestsFixture : IAsyncLifetime
    {
        private DistributedApplication app;
        public DistributedApplication App => app;
        public string ResourceName => "aroundu-events-service-api";

        public async ValueTask InitializeAsync()
        {
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.Aroundu_AppHost>();

            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler(options =>
                {
                    options.TotalRequestTimeout.Timeout = TimeSpan.FromMinutes(3);

                    options.Retry.MaxRetryAttempts = 10;
                    options.Retry.Delay = TimeSpan.FromSeconds(3);

                    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(3);

                    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(10);
                });

                clientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                });
            });

            app = await appHost.BuildAsync();
            await app.StartAsync();

            await app.ResourceNotifications.WaitForResourceHealthyAsync("sql-server");

            await app.ResourceNotifications.WaitForResourceHealthyAsync(ResourceName);
        }

        public async ValueTask DisposeAsync()
        {
            if (app != null)
            {
                await app.DisposeAsync();
            }
        }
    }
}
