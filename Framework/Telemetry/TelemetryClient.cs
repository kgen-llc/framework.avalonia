using System.Text;

namespace KGen.Framework.Telemetry;
public class TelemetryClient
{
    private static readonly Uri PlausibleApiUrl = new("https://plausible.io/api/event");
    private readonly string productName;
    private readonly HttpClient client;

    public static TelemetryClient? Instance { get; private set; }

    public static async Task InitTelemetry(string productName, string browser)
    {
        if(string.IsNullOrEmpty(productName))
        {
            throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));
        }

        if(!string.Equals(Environment.GetEnvironmentVariable("KGEN_NO_TELEMETRY"), "0", StringComparison.Ordinal) || !string.Equals(Environment.GetEnvironmentVariable($"KGEN_NO_TELEMETRY_{productName.ToUpperInvariant()}"), "0", StringComparison.Ordinal))
        {
            return;
        }
        if (Instance != null)
        {
            throw new InvalidOperationException("TelemetryClient is already initialized.");
        }

        Instance = new (productName, browser);

         await Instance.TrackPageView("Startup").ConfigureAwait(false);
    }

    public static async Task ExitTelemetry()
    {
        if(Instance == null)
        {
            return;
        }
        await Instance.TrackPageView("Exit").ConfigureAwait(false); 
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