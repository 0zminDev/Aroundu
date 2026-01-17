#nullable enable
using Microsoft.Playwright;

namespace Aroundu.E2ETests
{
    public class PlaywrightTest : IAsyncLifetime
    {
        protected IPlaywright Playwright { get; private set; }
        protected IBrowser Browser { get; private set; }
        protected IPage Page { get; private set; }

        public async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500
            });

            Page = await Browser.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            await Browser.CloseAsync();
            Playwright.Dispose();
        }
    }
}
