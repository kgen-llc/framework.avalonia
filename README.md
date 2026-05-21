# Framework Avalonia

A comprehensive base framework for KGen company applications built on [Avalonia](https://avaloniaui.net/), a modern cross-platform UI framework for .NET. This framework provides essential utilities, telemetry integration, and architectural patterns to accelerate development of desktop applications.

## Overview

Framework Avalonia provides:
- **Base Application Framework** - KGenApp utility for application identification and platform information
- **Telemetry Support** - Built-in telemetry infrastructure for diagnostics and monitoring
- **Cross-Platform Support** - Desktop application development for Windows, Linux, and macOS
- **MVVM Architecture** - Foundation for maintainable, testable applications
- **Demo Application** - Reference implementation showing best practices

## Getting Started

### Prerequisites

- .NET 8.0 or higher
- Visual Studio 2022, Rider, or other .NET IDE
- (Optional) Avalonia Extensions for your IDE

### Building the Framework

```bash
# Clone the repository
git clone https://github.com/kgen-llc/framework.avalonia.git
cd framework.avalonia

# Build the solution
dotnet build

# Run KGen repository analyzers (code quality checks)
dotnet tool exec kgen.analyzers.repository

# Run tests
dotnet test

# Run the demo application
dotnet run --project Framework.Avalonia.Demo
```

**Note:** The `kgen.analyzers.repository` tool performs KGen company-standard code analysis and should be run before committing changes. Ensure all analyzer warnings are addressed.

## Project Structure

```
framework.avalonia/
├── Framework/                      # Core framework library
│   ├── KGenApp.cs                 # Application identification utilities
│   └── Telemetry/                 # Telemetry infrastructure
├── Framework.Avalonia/            # Avalonia-specific framework components
├── Framework.Avalonia.Demo/       # Reference implementation
│   ├── Program.cs                 # Application entry point
│   ├── App.axaml                  # Application shell
│   ├── Views/                     # UI components
│   ├── ViewModels/                # View logic and state
│   └── Models/                    # Data models
└── README.md
```

## Usage

### Using KGenApp for Application Information

The `KGenApp` class provides utilities to identify your application and platform information:

```csharp
using KGen.Framework;

// Get your application's product name
var appName = KGenApp.ProductName;

// Get comprehensive platform information (includes version and OS details)
var platformInfo = KGenApp.PlatformInfo;
// Example output: "MyApp/v1.0.0 (Win32NT 10.0.19045)"

// Use in telemetry or user-agent strings
Console.WriteLine($"Running {KGenApp.PlatformInfo}");
```

### Creating a New Application

1. **Create a new project** inheriting from this framework:

```bash
dotnet new sln -n MyApplication
dotnet new avalonia.app -n MyApplication.Desktop
```

2. **Reference the framework packages** via NuGet:

```bash
dotnet add package kgen.Framework
dotnet add package kgen.Framework.Avalonia
```

Or add directly to your project file:

```xml
<ItemGroup>
    <PackageReference Include="kgen.Framework" Version="1.0.0" />
    <PackageReference Include="kgen.Framework.Avalonia" Version="1.0.0" />
</ItemGroup>
```

3. **Set your application metadata** (for KGenApp utilities):

```xml
<PropertyGroup>
    <AssemblyName>MyApplication</AssemblyName>
    <AssemblyVersion>1.0.0</AssemblyVersion>
    <Product>MyApplication</Product>
</PropertyGroup>
```

4. **Initialize your Avalonia application** with the framework:

```csharp
public static class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
```

### MVVM Pattern

The framework supports the MVVM (Model-View-ViewModel) architecture:

- **Views** - Avalonia UI components (XAML/Axaml)
- **ViewModels** - Application logic and state management
- **Models** - Data structures and business logic

Example structure from the demo application:

```
Views/
  ├── MainWindow.axaml
  ├── SomeView.axaml
ViewModels/
  ├── MainWindowViewModel.cs
  ├── SomeViewModel.cs
Models/
  └── DataModel.cs
```

### Telemetry Integration

The framework includes telemetry infrastructure in the `Telemetry` namespace. This can be used for:

- Application diagnostics
- User session tracking
- Performance monitoring
- Error reporting

Implement telemetry according to KGen company standards and privacy policies.

### Development Guidelines for KGen

#### Build Process

The standard KGen build process includes:

1. **dotnet build** - Compile the solution
2. **dotnet tool exec kgen.analyzers.repository** - Run code quality and compliance checks
3. **dotnet test** - Execute unit and integration tests

All three steps should be run before committing changes or submitting pull requests.

#### Code Standards

- Use C# 11+ language features
- Follow Microsoft C# naming conventions (PascalCase for public members, camelCase for private)
- Ensure XAML files use the `.axaml` extension (Avalonia convention)
- Include XML documentation for public APIs
- Use meaningful variable and method names

### Project Configuration

The solution uses Directory.Build.props for shared build settings:

- **Directory.Build.props** - Centralized MSBuild properties
- **Directory.Packages.props** - Centralized NuGet package versions

Update these files when adding new dependencies used across multiple projects.

### Dependencies

Key dependencies:

- **Avalonia** - Cross-platform UI framework
- **.NET Runtime** - Target .NET 8.0+

Check `Directory.Packages.props` for current version pins.

## Contributing

1. Create a feature branch from `main`
2. Make your changes following the code standards above
3. Test thoroughly with the demo application
4. Submit a pull request with clear description of changes

## Architecture

### Application Initialization Flow

1. `Program.Main()` entry point (STA thread for UI)
2. `BuildAvaloniaApp()` configures Avalonia
3. `App.xaml/App.axaml.cs` initializes the application shell
4. `MainWindow` displays the primary UI
5. `ViewLocator` binds Views to ViewModels automatically

### ViewLocator Pattern

The framework includes a `ViewLocator` that automatically resolves Views for ViewModels, enabling:

- Automatic binding of Views to ViewModels
- Clean separation of concerns
- Maintainable UI architecture

## Testing

Run unit tests and integration tests:

```bash
# Run all tests
dotnet test

# Run tests for a specific project
dotnet test Framework.Tests
```

## Deployment

### Building for Distribution

```bash
# Build release configuration
dotnet build -c Release

# Publish for a specific runtime
dotnet publish -c Release -r win-x64 --self-contained
```

For KGen deployment standards, see internal documentation.

## Troubleshooting

### Common Issues

- **XAML Designer not working** - Ensure Avalonia IDE extensions are installed
- **Platform detection fails** - Verify `usePlatformDetect()` is called in AppBuilder
- **Telemetry not initializing** - Check Telemetry namespace imports and configuration

### Getting Help

- Consult the [Avalonia Documentation](https://docs.avaloniaui.net/)
- Review the demo application for reference implementations
- Check internal KGen development wiki/documentation

## License

See LICENSE file for details.

## Contact

For KGen company-specific questions about this framework, contact the development team.
