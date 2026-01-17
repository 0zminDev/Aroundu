namespace Aroundu.Events.Service.IntegrationTests.Tests
{
    public class EventsApiIntegrationTests : IClassFixture<IntegrationTestsFixture>
    {
        private readonly IntegrationTestsFixture fixture;

        public EventsApiIntegrationTests(IntegrationTestsFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Get_Health_Should_Return_OK()
        {
            // Arrange
            using var client = fixture.App.CreateHttpClient(fixture.ResourceName);

            // Act
            var response = await client.GetAsync("/health");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_Events_Count_Should_Return_Success()
        {
            // Arrange
            var client = fixture.App.CreateHttpClient(fixture.ResourceName);

            // Act
            var response = await client.GetAsync("/api/events/count");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("eventCount", content, StringComparison.OrdinalIgnoreCase);
        }
    }
}
