namespace DataEntryDemo
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using DataEntryDemo.Data;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Playwright;

    public static class PlaywrightService
    {
        public static IPage page { get; set; }
        public static IBrowser _browser { get; set; }
        public static IBrowserContext context { get; set; }
        public static IPlaywright _playwright { get; set; }
        private static TaskCompletionSource<bool> _userClickedCreateButtonTcs = new();

        public static IPage GetPage() { return page; }   
        public static IBrowserContext GetBrowser() { return context; }

        public static async Task PerformAutomationStepsAsync (Car car)
        {
            var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false, // Keep browser visible for testing
            });
            _browser = browser;
            var context = await browser.NewContextAsync();
             page = await context.NewPageAsync();

            // Navigate to the webpage
            await page.GotoAsync("https://acc3inter01.pegalabs.io/prweb/PRServlet/app/default/beEBp4uRVTogorRwSwWqbOtn9IL2fwdI*/!STANDARD");

            // Perform Login
            await page.GetByPlaceholder("User name").FillAsync("wesley.reitz@acc3int.com");
            await page.GetByPlaceholder("Password", new() { Exact = true }).FillAsync("Install@1234");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

            // Navigate to Data Entry and fill the form
            await page.Locator("a").Filter(new() { HasTextRegex = new Regex("^Create$") }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Data Entry" }).ClickAsync();
            
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Name", Exact = true }).FillAsync("myname");
            await _userClickedCreateButtonTcs.Task;

        }

        public static async Task FillFormField (Car car)
        {
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Name", Exact = true }).FillAsync(car.Name);
            await page.GetByLabel("ReleaseDate").FillAsync(car?.ReleaseDate?.ToString("MM/dd/yyyy") ?? "");
            await page.GetByLabel("Color").FillAsync(car?.ColorId.ToString() ?? "");
            await page.GetByLabel("LastAvailableDate").FillAsync(car?.LastAvailableDate?.ToString("MM/dd/yyyy") ?? "");
            await page.GetByLabel("Year").FillAsync(car?.Year?.ToString() ?? "");
            await page.GetByLabel("Total").FillAsync(car?.Total?.ToString() ?? "");
            await page.GetByLabel("Model").FillAsync(car?.ModelId.ToString() ?? "");
            await page.GetByLabel("Owner").FillAsync(car?.Owner?.ToString() ?? "");
            await page.GetByLabel("Quantity").FillAsync(car?.Quantity?.ToString() ?? "");
            await page.GetByLabel("Status").FillAsync(car?.StatusId.ToString() ?? "");

            // Wait for the user to signal continuation
            await _userClickedCreateButtonTcs.Task;

            // Finish automation
            await page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();
            await page.GotoAsync("https://example.com");
        }

        public static void SignalUserClickedCreateButton ()
        {
            _userClickedCreateButtonTcs.TrySetResult(true);
        }

   
    }

}