using FluentAssertions;

namespace Aroundu.E2ETests
{
    public class E2ETest : PlaywrightTest
    {
        [Fact]
        public async Task Check_Google_Title()
        {
            // Arrange
            await Page.GotoAsync("https://www.google.com");

            // Act
            var title = await Page.TitleAsync();

            // Assert
            title.Should().Contain("Google");
        }
    }
}
