// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Runtime.Versioning;
using Microsoft.Win32;
using MineOrc.Foundation.Runtime.Java;

namespace MineOrc.Management.Runtime.Java;

[SupportedOSPlatform("windows")]
public sealed class JavaSoftJavaLocator : IJavaLocator
{
    public bool CanExecute()
    {
        return OperatingSystem.IsWindows();
    }

    public IEnumerable<KeyValuePair<string, JavaInfo>> SearchJava()
    {
        using var javaSoft = Registry.LocalMachine.OpenSubKey("SOFTWARE")
            ?.OpenSubKey("JavaSoft");
        if (javaSoft == null)
        {
            yield break;
        }

        using var jdk = javaSoft.OpenSubKey("JDK");
        using var jre = javaSoft.OpenSubKey("JRE");

        foreach (var x in EvaluateJavaInfos(jdk, "JDK")) yield return x;
        foreach (var x in EvaluateJavaInfos(jre, "JRE")) yield return x;
    }

    private static IEnumerable<KeyValuePair<string, JavaInfo>> EvaluateJavaInfos(
        RegistryKey? sourceKey,
        string type)
    {
        if (sourceKey == null)
        {
            yield break;
        }
        
        var names = sourceKey.GetSubKeyNames();
        foreach (var name in names
                     .Where(x => !x.Contains('.')))
        {
            var key = sourceKey.OpenSubKey(name);
            var javaHome = key?.GetValue("JavaHome") as string;
            if (!int.TryParse(name, out var majorVersion)
                || key == null
                || javaHome == null)
            {
                continue;
            }

            yield return new KeyValuePair<string, JavaInfo>(
                key: $"{type}-{majorVersion}_reg",
                value: new JavaInfo
                {
                    MajorVersion = majorVersion,
                    AutoAdded = true,
                    ExecutablePath = Path.Combine(javaHome, @"bin\java.exe"),
                });
        }
    }
}