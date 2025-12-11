// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using JetBrains.Annotations;
using MineOrc.Foundation.Instancing;
using MineOrc.Foundation.Network;
using MineOrc.Foundation.Runtime.Java;
using MineOrc.Foundation.Utilities;
using MineOrc.Instancing;
using MineOrc.Network.Security;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc;

public static class MineOrcApp
{
    public static readonly string BaseName = Path.GetFileNameWithoutExtension(Environment.ProcessPath)
        ?? Assembly.GetEntryAssembly()?.GetName().Name
        ?? "mineorc";
    
    public static readonly string Version = typeof(MineOrcApp).Assembly.GetName().Version?.ToString(3)
        ?? "0.0.0-unknown";

    private static readonly string JavaConfigPath = Path.Combine(MinecraftDirectory.UserRoot,
        "mineorc_java_runtimes.json");

    private static readonly string ProfilesPath = Path.Combine(MinecraftDirectory.UserRoot,
        "mineorc_profiles");
    
    public static readonly HttpClient HttpClient = new()
    {
        DefaultRequestHeaders =
        {
            UserAgent =
            {
                new ProductInfoHeaderValue(BaseName, Version)
            }
        }
    };

    public static readonly PistonMeta PistonMetaClient = new(HttpClient);

    public static readonly JavaRegistryManager JavaRegistry = new(JavaConfigPath);

    public static readonly AccountManager AccountManager = new();

    public static readonly ProfileManager ProfileManager = new(ProfilesPath);

    [MustUseReturnValue("Initialization may fail")]
    public static async Task<bool> InitializeAsync()
    {
        try
        {
            Directory.CreateDirectory(ProfilesPath);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MyOutput.Error(ex, Texts.InitializationDirectoryError);
            return false;
        }

        try
        {
            await JavaRegistry.LoadAsync().ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is IOException or JsonException)
        {
            MyOutput.Warn(ex, Texts.InitializationLoadRuntimesError);
        }

        return true;
    }
}