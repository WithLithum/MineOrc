// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Manifest.Options;
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

    public static ProcessStartInfo CreateStartInfo(string javaExecutable,
        VersionArguments arguments,
        IEnumerable<string> classPath,
        LaunchJvmSettings jvmSettings,
        LaunchGameSettings gameSettings)
    {
        var startInfo = new ProcessStartInfo(javaExecutable);

        // JVM
        if (jvmSettings.MinMemory.HasValue)
        {
            startInfo.ArgumentList.Add($"-Xms{jvmSettings.MinMemory:D}M");
        }
        
        if (jvmSettings.MaxMemory.HasValue)
        {
            startInfo.ArgumentList.Add($"-Xmx{jvmSettings.MaxMemory:D}M");
        }
        AppendJvmArguments(jvmSettings, classPath, arguments.Jvm, startInfo.ArgumentList);
        
        // Main class
        startInfo.ArgumentList.Add(jvmSettings.MainClass);
        
        // Game
        AppendGameArguments(gameSettings, arguments.Game, startInfo.ArgumentList);

        return startInfo;
    }

    private static void AppendJvmArguments(LaunchJvmSettings settings,
        IEnumerable<string> classPath,
        IEnumerable<JvmArgumentEntry> entries,
        Collection<string> addTo)
    {
        var resolver = CreateJvmResolver(settings, classPath);

        foreach (var entry in entries
                     .Where(x => x.Rules == null || ArgumentConditions.CheckApplies(x.Rules))
                     .SelectMany(x => x.Value))
        {
            addTo.Add(resolver.Resolve(entry));
        }
    }

    public static ArgumentValueResolver CreateJvmResolver(LaunchJvmSettings settings,
        IEnumerable<string> classpath)
    {
        var dict = new Dictionary<string, string>
        {
            { "natives_directory", settings.NativesDirectory },
            { "launcher_name", settings.LauncherBrand },
            { "launcher_version", settings.LauncherVersion },
            { "classpath", AssembleClasspath(classpath) },
        };

        return new ArgumentValueResolver(dict);
    }

    private static void AppendGameArguments(LaunchGameSettings settings,
        IEnumerable<GameArgumentEntry> entries,
        Collection<string> addTo)
    {
        var resolver = CreateGameResolver(settings);
        var features = GetGameFeatures(settings);
        foreach (var entry in entries
                     .Where(x =>
                         x.Rules == null || ArgumentConditions.CheckApplies(x.Rules, features))
                     .SelectMany(x => x.Value))
        {
            addTo.Add(resolver.Resolve(entry));
        }
    }

    private static IReadOnlyCollection<string> GetGameFeatures(LaunchGameSettings settings)
    {
        var list = new List<string>();
        list.AddIf(settings.IsDemoMode, GameArgumentFeature.IsDemoUser);
        list.AddIf(settings.CustomResolution.HasValue, GameArgumentFeature.HasCustomResolution);
        list.AddIf(settings.QuickPlayPath != null, GameArgumentFeature.HasQuickPlaysSupport);
        list.AddIf(settings.QuickPlaySingleplayer != null,
            GameArgumentFeature.IsQuickPlaySingleplayer);
        list.AddIf(settings.QuickPlayMultiplayer != null,
            GameArgumentFeature.IsQuickPlayMultiplayer);
        list.AddIf(settings.QuickPlayRealms != null, GameArgumentFeature.IsQuickPlayRealms);

        return list;
    }

    public static ArgumentValueResolver CreateGameResolver(LaunchGameSettings settings)
    {
        var session = settings.Session.Session;
        var profile = settings.Session.Profile;

        var dict = new Dictionary<string, string>
        {
            { "auth_player_name", profile.Name },
            { "auth_uuid", profile.Id.ToString("N") },
            { "auth_access_token", session.AccessToken },
            { "clientid", settings.ClientId },
            { "user_type", "msa" },
            { "assets_index_name", settings.AssetsVersion },
            { "assets_root", settings.AssetsRoot },
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

        dict.AddIfNotNull("auth_xuid", settings.Session.XboxId);
        dict.AddIfNotNull("quickPlayPath", settings.QuickPlayPath);
        dict.AddIfNotNull("quickPlaySingleplayer", settings.QuickPlaySingleplayer);
        dict.AddIfNotNull("quickPlayMultiplayer", settings.QuickPlayMultiplayer);
        dict.AddIfNotNull("quickPlayRealms", settings.QuickPlayRealms);

        return new ArgumentValueResolver(dict);
    }
}