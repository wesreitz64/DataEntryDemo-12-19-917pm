using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DataEntryDemo.Data;
using Microsoft.Playwright;

public class PlaywrightService
{
    private TaskCompletionSource<bool> _userClickedCreateButtonTcs = new();
   
    public PlaywrightService () { }

    public async Task ExecuteFormAutomationAsync (CarModel carData)
    {
        var retryCount = 0;
        const int maxRetries = 2;

        while (true)
        {
            try
            {
                await PerformAutomationStepsAsync(carData);
                break; // Exit loop if successful
            }
            catch (Exception ex)
            {
                retryCount++;
                Console.WriteLine($"Attempt {retryCount} failed: {ex.Message}");

                if (retryCount > maxRetries)
                {
                    Console.WriteLine("Max retry attempts reached. Aborting automation.");
                    throw;
                }

                Console.WriteLine("Retrying...");
            }
        }
    }

    private async Task PerformAutomationStepsAsync (CarModel carData)
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
        await PerformLogin(page);

        // Navigate to Data Entry and fill the form
        await NavigateToDataForm(page);

        // Fill form fields
        await FillInForm(page,carData);

        // Wait for the user to signal continuation
        await _userClickedCreateButtonTcs.Task;

        // Finish automation
       // await SubmitFilledInForm(page);
    }

    private static async Task SubmitFilledInForm (IPage page)
    {
        await page.GetByRole(AriaRole.Button, new() { Name = "Create" }).ClickAsync();
        await page.GotoAsync("https://example.com");
    }

    private static async Task FillInForm (IPage page, CarModel? carData)
    {
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Name", Exact = true }).FillAsync(carData!.Car!.Name);
        string releaseFormattedDate = carData.Car.ReleaseDate.GetValueOrDefault().ToString("MM/dd/yyyy");
        await page.GetByLabel("ReleaseDate").FillAsync(releaseFormattedDate);
        await page.GetByLabel("Color").FillAsync(carData!.Car!.ColorId.ToString());
        string lastFormattedDate = carData.Car.LastAvailableDate.GetValueOrDefault().ToString("MM/dd/yyyy");
        await page.GetByLabel("LastAvailableDate").FillAsync(lastFormattedDate);
        await page.GetByLabel("Year").FillAsync(carData!.Car!.Year!.ToString());
        string formattedTotal = carData!.Car!.Total!.ToString(  );
        await page.GetByLabel("Total").FillAsync(formattedTotal);
        await page.GetByLabel("Model").FillAsync(carData.Car.ModelId.ToString());
        await page.GetByLabel("Owner").FillAsync(carData!.Car!.Owner!);
        await page.GetByLabel("Quantity").FillAsync(carData!.Car!.Quantity!.ToString());
        await page.GetByLabel("Status").FillAsync(carData!.Car!.StatusId!.ToString());
    }

    private static async Task NavigateToDataForm (IPage page)
    {
        await page.Locator("a").Filter(new() { HasTextRegex = new Regex("^Create$") }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "Data Entry" }).ClickAsync();
    }

    private static async Task PerformLogin (IPage page)
    {
        await page.GetByPlaceholder("User name").FillAsync("wesley.reitz@acc3int.com");
        await page.GetByPlaceholder("Password", new() { Exact = true }).FillAsync("Install@1234");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    }

    public void SignalUserClickedCreateButton ()
    {
        _userClickedCreateButtonTcs.TrySetResult(true);
    }
}
