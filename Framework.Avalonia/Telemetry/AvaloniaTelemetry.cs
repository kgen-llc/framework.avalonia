using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace KGen.Framework.Avalonia.Telemetry;

public static class AvaloniaTelemetry 
{
    public static async Task InitTelemetry(string productName, string browser)
    {
        await Framework.Telemetry.TelemetryClient.InitTelemetry(productName, browser).ConfigureAwait(false);
        var lifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
        lifetime.Exit += (sender, e) =>
        {
            Framework.Telemetry.TelemetryClient.ExitTelemetry().Wait();
        };
}
}
