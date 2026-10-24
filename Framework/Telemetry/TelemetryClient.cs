using System.Text;
using System.Text.Json;

namespace KGen.Framework.Telemetry;
public class TelemetryClient
{
    private static readonly Uri PlausibleApiUrl = new("https://plausible.io/api/event");
    private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);
    private static readonly HttpClient SharedClient = CreateHttpClient();
    private readonly string productName;

    public static TelemetryClient? Instance { get; private set; }

    public static async Task InitTelemetry(string productName, string browser)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ArgumentException("Product name cannot be null or empty.", nameof(productName));
        }

        if (string.Equals(Environment.GetEnvironmentVariable("KGEN_NO_TELEMETRY"), "1", StringComparison.Ordinal) || string.Equals(Environment.GetEnvironmentVariable($"KGEN_NO_TELEMETRY_{productName.ToUpperInvariant()}"), "1", StringComparison.Ordinal))
        {
            return;
        }
        if (Instance != null)
        {
            throw new InvalidOperationException("TelemetryClient is already initialized.");
        }

        Instance = new(productName, browser);

        await Instance.TrackPageView("Startup").ConfigureAwait(false);
    }

    public static async Task ExitTelemetry()
    {
        if (Instance == null)
        {
            return;
        }

#pragma warning disable CA1031 // Do not catch general exception types - we do not want any exception if telemetry is off
        try
        {
            await Instance.TrackPageView("Exit").ConfigureAwait(false);
        }
        catch
        {
            // Telemetry is best-effort only.
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    protected TelemetryClient(string productName, string browser)
    {
        ArgumentNullException.ThrowIfNull(productName);
        ArgumentNullException.ThrowIfNull(browser);

        this.productName = productName.Trim();
        SharedClient.DefaultRequestHeaders.Remove("User-Agent");
        SharedClient.DefaultRequestHeaders.Add("User-Agent", TelemetryUserAgent.GenerateUserAgent(browser));
    }

    public async Task<bool> TrackPageView(string pageName)
    {
        if (string.IsNullOrWhiteSpace(pageName))
        {
            return false;
        }

        var payload = new
        {
            name = "pageview",
            url = $"app://kgen-llc.com/{EscapeSegment(productName)}/{EscapeSegment(pageName)}",
            domain = "kgen-llc.com"
        };

        using var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

#pragma warning disable CA1031 // Do not catch general exception types - we do not want any exception if telemetry is off
        try
        {
            using var cts = new CancellationTokenSource(RequestTimeout);
            var response = await SharedClient.PostAsync(PlausibleApiUrl, content, cts.Token).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private static HttpClient CreateHttpClient()
    {
        return new HttpClient
        {
            Timeout = RequestTimeout
        };
    }

    private static string EscapeSegment(string value)
    {
        return Uri.EscapeDataString(value.Trim());
    }
}
