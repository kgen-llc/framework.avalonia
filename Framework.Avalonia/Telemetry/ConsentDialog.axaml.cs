using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Framework.Avalonia;

namespace KGen.Framework.Avalonia.Telemetry;

public partial class ConsentDialog : Window
{
    private readonly Func<Window> mainWindowFactory;

    public ConsentDialog(Func<Window> mainWindowFactory)
    {
        InitializeComponent();
        this.Title = KGenApp.ProductName;

        this.mainWindowFactory = mainWindowFactory;
    }

    public void YesButtonClick(object sender, RoutedEventArgs args)
    {
#if !DEBUG
        _ = TelemetryClient.InitTelemetry(KGenApp.ProductName, KGenApp.PlatformInfo); // we only init the telemetry when the user click yes and in debug
#endif
        var desktop = (IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!;
     
        desktop.MainWindow = mainWindowFactory();

        desktop.MainWindow.Show();

        Close();
    }

    public void NoButtonClick(object sender, RoutedEventArgs args)
    {
        Close();
    }
}
