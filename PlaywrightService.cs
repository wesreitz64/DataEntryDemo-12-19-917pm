namespace DataEntryDemo
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Microsoft.Playwright;

    public class PlaywrightService
    {
        public IPage _page { get; set; }
        public IBrowser _browser { get; set; }
        public IBrowserContext browserContext { get; set; }
        public IPlaywright _playwright { get; set; }
        private TaskCompletionSource<bool> _userClickedCreateButtonTcs = new();


        public async Task PerformAutomationStepsAsync ()
        {
            var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // Keep browser visible for testing
            });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            // Navigate to the webpage
            await page.GotoAsync("https://acc3inter01.pegalabs.io/prweb/PRServlet/app/default/beEBp4uRVTogorRwSwWqbOtn9IL2fwdI*/!STANDARD");

            // Perform Login
            await page.GetByPlaceholder("User name").FillAsync("wesley.reitz@acc3int.com");
            await page.GetByPlaceholder("Password", new() { Exact = true }).FillAsync("Install@1234");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

            // Navigate to Data Entry and fill the form
            await page.Locator("a").Filter(new() { HasTextRegex = new Regex("^Create$") }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Data Entry" }).ClickAsync();

            // Fill form fields
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Name", Exact = true }).FillAsync("name");
            await page.GetByLabel("ReleaseDate").FillAsync("11/24/2024");
            await page.GetByLabel("Color").FillAsync("red");
            await page.GetByLabel("LastAvailableDate").FillAsync("12/31/2025");
            await page.GetByLabel("Year").FillAsync("2025");
            await page.GetByLabel("Total").FillAsync("150,000");
            await page.GetByLabel("Model").FillAsync("Mustang");
            await page.GetByLabel("Owner").FillAsync("Fred Barnes");
            await page.GetByLabel("Quantity").FillAsync("3");
            await page.GetByLabel("Status").FillAsync("Like New");

            // Wait for the user to signal continuation
            await _userClickedCreateButtonTcs.Task;

            // Finish automation
            await page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();
            await page.GotoAsync("https://example.com");
        }
        public void SignalUserClickedCreateButton ()
        {
            _userClickedCreateButtonTcs.TrySetResult(true);
        }

   
    }

}