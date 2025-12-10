// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Runtime.Launch;
using MineOrc.Foundation.Utilities;

namespace MineOrc.Foundation.Runtime.Arguments;

public static class ArgumentAssembler
{
    public static string AssembleClasspath(IEnumerable<string> classpath)
    {
        var builder = new StringBuilder();
        var first = false;
        foreach (var path in classpath)
        {
            if (!first)
            {
                first = true;
            }
            else
            {
                builder.Append(Path.PathSeparator);
            }
            
            builder.Append(path);
        }

        return builder.ToString();
    }
    
    public static ArgumentValueResolver CreateJvmResolver(LaunchJvmSettings settings)
    {
        var dict = new Dictionary<string, string>
        {
            { "natives_directory", settings.NativesDirectory },
            { "launcher_name", settings.LauncherBrand },
            { "launcher_version", settings.LauncherVersion },
        };

        return new ArgumentValueResolver(dict);
    }
    
    public static ArgumentValueResolver CreateGameResolver(LaunchGameSettings settings)
    {
        var auth = settings.AuthenticationResult;
        if (!auth.Succeeded)
        {
            throw new ArgumentException("The authentication result does not indicate success.",
                nameof(settings));
        }

        var dict = new Dictionary<string, string>
        {
            { "auth_player_name", auth.Profile.UserName },
            { "auth_uuid", auth.OverrideId ?? auth.Profile.Id.ToString("N") },
            { "auth_access_token", auth.AccessToken },
            { "auth_xuid", auth.Xuid },
            { "clientId", settings.ClientId },
            { "user_type", auth.UserType },
            { "assets_index_name", settings.AssetsVersion },
            { "assets_dir", settings.AssetsRoot },
            { "game_directory", settings.GameDirectory },
            { "version_name", settings.VersionName },
            { "version_type", VersionTypes.ToString(settings.VersionType) },
        };

        if (settings.CustomResolution.HasValue)
        {
            var size = settings.CustomResolution.Value;
            dict.Add("resolution_width", size.Width.ToString("D"));
            dict.Add("resolution_height", size.Height.ToString("D"));
        }
        
        dict.AddIfNotNull("quickPlayPath", settings.QuickPlayPath);
        dict.AddIfNotNull("quickPlaySingleplayer", settings.QuickPlaySingleplayer);
        dict.AddIfNotNull("quickPlayMultiplayer", settings.QuickPlayMultiplayer);
        dict.AddIfNotNull("quickPlayRealms", settings.QuickPlayRealms);

        return new ArgumentValueResolver(dict);
    }
}