using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using Microsoft.Playwright;

public  class PlaywrightService
{
    private IPage _page;

    public async Task InitializeBrowserAsync ()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false // Keep browser visible for testing
        });

        var context = await browser.NewContextAsync();
        _page = await context.NewPageAsync();
    }


    public async Task PerformManualLoginAsync (string loginUrl)
    {
        if (_page == null) throw new InvalidOperationException("Browser not initialized.");

        // Navigate to the login page
        await _page.GotoAsync(loginUrl);

        Console.WriteLine("Waiting for user to log in...");

        await _page.WaitForURLAsync(url => url != loginUrl, new() { Timeout = 60000 });

        Console.WriteLine("Login successful. Navigated to a new page.");

        // Call NavigateToDataFormAsync after login and navigation
        await NavigateToDataFormAsync();
    }


    public async Task NavigateAndLoginAsync (string url, string username, string password)
    {
        if (_page == null) throw new InvalidOperationException("Browser not initialized.");

        await _page.GotoAsync(url);

        // Perform Login
        await _page.GetByPlaceholder("User name").FillAsync(username);
        await _page.GetByPlaceholder("Password", new() { Exact = true }).FillAsync(password);
        await _page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    }

    public async Task NavigateToDataFormAsync ()
    {
        if (_page == null) throw new InvalidOperationException("Browser not initialized.");

        await _page.Locator("a").Filter(new() { HasTextRegex = new Regex("^Create$") }).ClickAsync();
        await _page.GetByRole(AriaRole.Link, new() { Name = "Data Entry" }).ClickAsync();
    }

    public async Task FillInFormAsync (DataEntryDemo.Pages.Index.Car car)
    {
        if (_page == null) throw new InvalidOperationException("Browser not initialized.");

        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Name", Exact = true }).FillAsync(car.Name);
        string releaseFormattedDate = car.ReleaseDate.GetValueOrDefault().ToString("MM/dd/yyyy");
        await _page.GetByLabel("ReleaseDate").FillAsync(releaseFormattedDate);
        // Repeat for other fields...
    }
}

