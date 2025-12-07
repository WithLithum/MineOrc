// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Utilities;

public static class MinecraftDirectory
{
    public static readonly string UserRoot = GetUserRoot();

    public static readonly string UserAssets = Path.Combine(UserRoot,
        "assets");
    public static readonly string UserLibraries = Path.Combine(UserRoot,
        "libraries");
    public static readonly string UserVersions = Path.Combine(UserRoot,
        "versions");

    private static string GetUserRoot()
    {
        if (OperatingSystem.IsWindows())
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ".minecraft");
        }

        // Unix systems
        
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (OperatingSystem.IsMacOS())
        {
            return Path.Combine(userProfile,
                "Library",
                "Application Support",
                "minecraft");
        }
        
        // All the rest of POSIX systems uses .minecraft anyway

        return Path.Combine(userProfile, ".minecraft");
    }
}