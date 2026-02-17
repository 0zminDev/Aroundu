#nullable enable
using Microsoft.Playwright;

namespace Aroundu.E2ETests
{
    public class PlaywrightTest : IAsyncLifetime
    {
        public required IPlaywright Playwright { get; set; }
        public required IBrowser Browser { get; set; }
        public required IPage Page { get; set; }

        public async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            Browser = await Playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions { Headless = false, SlowMo = 500 }
            );

            Page = await Browser.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            await Browser.CloseAsync();
            Playwright.Dispose();
        }
    }
}
