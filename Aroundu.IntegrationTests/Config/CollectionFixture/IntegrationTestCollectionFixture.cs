using Aroundu.IntegrationTests.Respawn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aroundu.IntegrationTests.CollectionFixture
{
    [CollectionDefinition("System Integration")]
    public class SystemCollection : ICollectionFixture<ApplicationTestingFactory> { }

    [Collection("System Integration")]
    public abstract class BaseIntegrationTest
    {
        protected readonly ApplicationTestingFactory Factory;
        protected readonly HttpClient Client;

        protected BaseIntegrationTest(ApplicationTestingFactory factory)
        {
            Factory = factory;
            Client = factory.GatewayClient;
        }

        protected async Task CleanupDatabasesAsync()
        {
            var eventsCs = await Factory.GetConnectionString("events-db");
            var authCs = await Factory.GetConnectionString("auth-db");

            await IntegrationTestDbCleanup.ResetAsync(eventsCs);
            await IntegrationTestDbCleanup.ResetAsync(authCs);
        }
    }
}
