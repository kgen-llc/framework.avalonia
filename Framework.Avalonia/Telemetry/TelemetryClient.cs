using System.Text;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace KGen.Framework.Avalonia.Telemetry;

public class TelemetryClient
{
    private static readonly Uri PlausibleApiUrl = new("https://plausible.io/api/event");
    private readonly string productName;
    private readonly HttpClient client;

    public static TelemetryClient? Instance { get; private set; }

    public static async Task InitTelemetry(string productName, string browser)
    {
        if (Instance != null)
        {
            throw new InvalidOperationException("TelemetryClient is already initialized.");
        }

        Instance = new (productName, browser);

         var lifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
            lifetime.Exit += (sender, e) =>
            {
                Instance.TrackPageView("Exit").Wait(); 
            };

         await Instance.TrackPageView("Startup").ConfigureAwait(false);
    }

    protected TelemetryClient(string productName, string browser)
    {
        client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", TelemetryUserAgent.GenerateUserAgent(browser));
        this.productName = productName;
    }

    public async Task<bool> TrackPageView(string pageName)
    {
        using var content = new StringContent(
            $"{{\"name\":\"pageview\",\"url\":\"app://{productName}/{pageName}\",\"domain\":\"kgen-llc.com\"}}", 
            Encoding.UTF8, "application/json");

#pragma warning disable CA1031 // Do not catch general exception types - we do not want any exception if telemetry is off
        try
        {
            var response = await client.PostAsync(PlausibleApiUrl, content).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

}