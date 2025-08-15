using System.Runtime.InteropServices;

namespace KGen.Framework.Avalonia.Telemetry;

public static class TelemetryUserAgent
{
    public static string GenerateUserAgent(string browser)
    {
        string osPlatform = GetOperatingSystem();
        string osVersion = GetOperatingSystemVersion();
        string architecture = GetArchitecture();

        return $"{browser} {osPlatform} {osVersion} ({architecture})";
    }
    public static string GetOperatingSystem()
    {
        if (OperatingSystem.IsWindows())
            return "Windows";
        if (OperatingSystem.IsLinux())
            return "Linux";
        if (OperatingSystem.IsMacOS())
            return "macOS";
        if (OperatingSystem.IsBrowser())
            return "Browser";

        return "Unknown";
    }

    public static string GetOperatingSystemVersion()
    {
        if (OperatingSystem.IsWindows())
            return Environment.OSVersion.VersionString;
        if (OperatingSystem.IsLinux())
            return GetLinuxDistributionVersion();
        if (OperatingSystem.IsMacOS())
            return GetMacOSVersion();

        return "Unknown";
    }

    public static string GetArchitecture()
    {
        return RuntimeInformation.ProcessArchitecture.ToString(); // x86, x64, ARM
    }

    private static string GetLinuxDistributionVersion()
    {
#pragma warning disable CA1031 // Do not catch general exception types - we do not want any exception if telemetry is off
        try
        {
            // This works for many Linux distributions by reading /etc/os-release
            string[] osReleaseInfo = File.ReadAllLines("/etc/os-release");
            foreach (var line in osReleaseInfo)
            {
                if (line.StartsWith("VERSION=", StringComparison.OrdinalIgnoreCase))
                {
                    return line.Split('=')[1].Trim('"');
                }
            }
            return "Unknown";
        }
        catch
        {
            return "Unknown";
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private static string GetMacOSVersion()
    {
#pragma warning disable CA1031 // Do not catch general exception types - we do not want any exception if telemetry is off
        try
        {
            // For macOS, we can fetch the version using the 'sw_vers' command
            var versionInfo = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "sw_vers",
                Arguments = "-productVersion",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            versionInfo!.WaitForExit();
            return versionInfo.StandardOutput.ReadToEnd().Trim();
        }
        catch
        {
            return "Unknown macOS Version";
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
}
