using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Framework.Avalonia.Demo.ViewModels;
using Framework.Avalonia.Demo.Views;
using KGen.Framework.Avalonia.Telemetry;

namespace Framework.Avalonia.Demo;

internal sealed partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new ConsentDialog(() => new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            });
        }

        base.OnFrameworkInitializationCompleted();
    }
}