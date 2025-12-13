// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using JetBrains.Annotations;
using MineOrc.Foundation.Network;
using MineOrc.Foundation.Utilities;
using MineOrc.Management.Profiles;
using MineOrc.Management.Runtime.Java;
using MineOrc.Network.Security;
using MineOrc.Resources;
using MineOrc.Security.Msa;

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

    public static readonly ApplicationMeta Meta = new()
    {
        Name = nameof(MineOrc),
        Version = Version,
        Package = "x.withlithum.mineorc",
    };

    internal static SecureAuthority SecureAuthority
    {
        get => field ?? throw new InvalidOperationException();
        private set;
    }

    internal static SecretModel Secrets
    {
        get => field ?? throw new InvalidOperationException("Initialization was not yet run.");
        private set;
    }
    
    public static readonly HttpClient HttpClient = new()
    {
        DefaultRequestHeaders =
        {
            UserAgent =
            {
                new ProductInfoHeaderValue(BaseName, Version),
            },
        },
    };

    public static readonly PistonMeta PistonMetaClient = new(HttpClient);

    public static readonly JavaRegistryManager JavaRegistry = new(JavaConfigPath);

    public static readonly AccountManager AccountManager = new();

    public static readonly ProfileManager ProfileManager = new(ProfilesPath);

    #region Initialization routine
    
    [MustUseReturnValue("Load may fail")]
    private static async Task<bool> LoadSecretsAsync()
    {
        // Load secrets
        var secretStream = typeof(MineOrcApp).Assembly
            .GetManifestResourceStream($"{nameof(MineOrc)}.Resources.Secrets.json");
        if (secretStream == null)
        {
            MyOutput.Error(Texts.InitializationSecretsNull);
            return false;
        }
        
        SecretModel? temp;
        try
        {
            await using (secretStream!.ConfigureAwait(false))
            {
                temp = await JsonSerializer.DeserializeAsync(secretStream!,
                    ResourceJsonContext.Default.SecretModel).ConfigureAwait(false);
            }
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            MyOutput.Error(ex, Texts.InitializationSecretsError);
            return false;
        }

        if (temp == null)
        {
            MyOutput.Error(Texts.InitializationSecretsNull);
            return false;
        }

        Secrets = temp;

        // Warn about secrets being empty
        if (Secrets.EntraAppId == "missingno" || Secrets.TenantId == "missingno")
        {
            MyOutput.Warn(Texts.InitializationWarnTenantMissing);
        }

        SecureAuthority = new SecureAuthority(Secrets.EntraAppId, Meta);
        await SecureAuthority.InitializeAsync().ConfigureAwait(false);
        
        return true;
    }
    
    [MustUseReturnValue("Initialization may fail")]
    public static async Task<bool> InitializeAsync()
    {
        // Load secrets
        if (!await LoadSecretsAsync().ConfigureAwait(false))
        {
            return false;
        }
        
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
    
    #endregion
}