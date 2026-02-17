using System.Net.Http.Json;
using Aroundu.Events.Service.Application.Commands;
using Aroundu.IntegrationTests.CollectionFixture;
using Shouldly;

namespace Aroundu.IntegrationTests.Tests.ServiceScope
{
    public class EventsServiceIntegrationTests : BaseIntegrationTest
    {
        public EventsServiceIntegrationTests(ApplicationTestingFactory factory)
            : base(factory) { }

        private record EventCountResponse(int EventCount);

        [Fact]
        public async Task CreateEvent_Should_StoreInDb_And_BeQueryable()
        {
            await CleanupDatabasesAsync();
            var command = new CreateEventCommand { Name = "New Event" };

            var response = await Client.PostAsJsonAsync("/api/events", command);

            response.EnsureSuccessStatusCode();
            var resultId = await response.Content.ReadFromJsonAsync<Guid>();
            resultId.ShouldNotBe(Guid.Empty);

            var countResponse = await Client.GetAsync("/api/events/count");
            var content = await countResponse.Content.ReadFromJsonAsync<EventCountResponse>();
            content.ShouldNotBeNull();
            ((int)content.EventCount).ShouldBe(1);
        }

        [Fact]
        public async Task CreateEvent_WithInvalidName_ShouldReturnBadRequest()
        {
            var command = new CreateEventCommand { Name = "" };

            var response = await Client.PostAsJsonAsync("/api/events", command);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
