using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;

using Framework.Avalonia;

namespace KGen.Framework.Avalonia.Telemetry;

internal sealed partial class ContactUs : UserControl
{
    public ContactUs()
    {
        InitializeComponent();
    }

    public void ContactUsClick(object? sender, PointerPressedEventArgs e) 
    {
        _ = Framework.Telemetry.TelemetryClient.Instance?.TrackPageView("ContactUs");
        OpenMailClient("tech@kgen-llc.com", $"Information regarding the ${KGenApp.ProductName} ", "Could we get in touch ?");
    }

    public static void OpenMailClient(string toEmail, string subject, string body)
    {
#pragma warning disable CA1031 // Do not catch general exception types - we do not want to propagate any exception if we cannot launch the email
        try
        {
            // Construct the mailto URI
            string mailtoUri = $"mailto:{toEmail}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";

            // Start the default email client
            Process.Start(new ProcessStartInfo(mailtoUri) { UseShellExecute = true });
        }
        catch
        {
            // we do not want to crash if we could not the email !
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

}