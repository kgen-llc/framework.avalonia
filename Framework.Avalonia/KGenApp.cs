using System;
using System.Reflection;

namespace Framework.Avalonia;

public static class KGenApp
{
    public static string? ProductName { get; set; } = Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyProductAttribute>()?.Product;

    public static string? PlatformInfo { get; set; } = $"{Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyProductAttribute>()?.Product}/v{Assembly.GetEntryAssembly()!.GetName().Version?.ToString(3)} ({Environment.OSVersion.Platform} {Environment.OSVersion.Version})";
}
