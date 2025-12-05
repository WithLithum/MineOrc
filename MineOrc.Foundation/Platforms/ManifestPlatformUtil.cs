using System.Runtime.InteropServices;

namespace MineOrc.Foundation.Platforms;

public static class ManifestPlatformUtil
{
    public static string GetSystemName()
    {
        if (OperatingSystem.IsWindows())
        {
            return "windows";
        }
        
        if (OperatingSystem.IsLinux())
        {
            return "linux";
        }
        
        if (OperatingSystem.IsMacOS())
        {
            return "osx";
        }

        return "unknown";
    }

    public static string GetSystemArch()
    {
        var arch = RuntimeInformation.OSArchitecture;
        return arch switch
        {
            Architecture.X86 => "x86",
            Architecture.X64 => "x64",
            Architecture.Arm => "arm",
            Architecture.Arm64 => "arm64",
            _ => arch.ToString().ToLowerInvariant()
        };
    }
}