// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Foundation.Instancing;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Network.Security;
using MineOrc.Foundation.Runtime.Arguments;
using MineOrc.Foundation.Runtime.Launch;
using MineOrc.Instancing.Operations;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Commands;

internal partial class LaunchCommand
{
    private const int DefaultMaxMemory = 2048;

    // Command state
    private string? _nativesDirectory;
    private string? _profileDirectory;

    private ProfileInfo? _profile;
    private ClientManifest? _version;
    private MinecraftAuthResult? _auth;
    private string? _javaCommand;

    private IEnumerable<string>? _classPath;
    private LaunchGameSettings? _gameSettings;
    private LaunchJvmSettings? _jvmSettings;

    // Arguments

    public static Command CreateCommand()
    {
        var command = new Command("launch", Texts.CommandLaunch)
        {
            ArgumentName,
            OptionDemo,
            OptionNoRestore,
            OptionMinMemory,
            OptionMaxMemory,
            OptionJavaName,
            OptionJavaExecutable,
            #if DEBUG
            DevAuth,
            #endif
        };

        command.SetAction(async (parse, cancel) =>
        {
            var obj = new LaunchCommand
            {
                ProfileName = parse.GetRequiredValue(ArgumentName),
                Demo = parse.GetValue(OptionDemo),
                NoRestore = parse.GetValue(OptionNoRestore),
                MinMemory = parse.GetValue(OptionMinMemory),
                MaxMemory = parse.GetValue(OptionMaxMemory),
                UserJavaName = parse.GetValue(OptionJavaName),
                UserJavaExecutable = parse.GetValue(OptionJavaExecutable),
                #if DEBUG
                UseDevAuth = parse.GetValue(DevAuth),                
                #endif
            };

            return await obj.ExecuteAsync(cancel).ConfigureAwait(false);
        });

        return command;
    }

    private async Task<int> ExecuteAsync(CancellationToken cancellationToken)
    {
        // Warning for dev users
        if (UseDevAuth)
        {
            AnsiConsole.MarkupLine("[bold underline white on red]DEV AUTHENTICATION ENABLED[/]");
            AnsiConsole.MarkupLine("[white]You have enabled[/] [bold white]Dev Authentication[/][white].[/]");
            AnsiConsole.MarkupLine("[white]This can only be used for testing purposes in development. The[/]");
            AnsiConsole.MarkupLine("[white]authors of MineOrc are NOT responsible for any use of the Dev[/]");
            AnsiConsole.MarkupLine("[white]Authentication for unlawful purposes.[/]");
            AnsiConsole.WriteLine();
        }
        
        // Profile & version & restore & login
        if (!await ExecuteProfileStepAsync().ConfigureAwait(false)
            || !await ExecuteVersionStepAsync(cancellationToken).ConfigureAwait(false)
            || !await ExecuteRestoreStepAsync(cancellationToken).ConfigureAwait(false)
            || !await ExecuteAuthenticationStepAsync(cancellationToken).ConfigureAwait(false)
            || !ExecuteJavaStep())
        {
            return ExitCodes.Failure;
        }

        // Libraries
        await ExecuteClassPathStepAsync(cancellationToken).ConfigureAwait(false);

        // Natives & settings
        if (!ExecuteNativeStep())
        {
            return ExitCodes.Failure;
        }

        ExecuteSettingsStep();

        // Start!
        var startInfo = ArgumentAssembler.CreateStartInfo(_javaCommand,
            _version!.Arguments,
            _classPath!,
            _jvmSettings,
            _gameSettings
        );

        return await ExecuteLaunchStepAsync(startInfo, cancellationToken).ConfigureAwait(false);
    }

    private static string? GetJava(string? javaName)
    {
        javaName ??= MineOrcApp.JavaRegistry.DefaultKey;
        if (string.IsNullOrWhiteSpace(javaName) ||
            !MineOrcApp.JavaRegistry.Items.TryGetValue(javaName, out var info)
            || !File.Exists(info.ExecutablePath))
        {
            MyOutput.Error(Texts.CommandLaunchFailNoJava);
            return null;
        }

        return info.ExecutablePath;
    }

    private async Task<MinecraftAuthResult?> AuthenticateAsync(CancellationToken cancellationToken)
    {
        IAuthenticationSource authSource;
        if (Demo)
        {
            authSource = new DemoAuthenticationSource();
        }
        else if (UseDevAuth)
        {
            authSource = new OfflineAuthenticationSource("MineOrcDev");
        }
        else
        {
            var tempSource = GetDefaultAuthenticationInternal();
            if (tempSource == null)
            {
                return null;
            }

            authSource = tempSource;
        }

        return await authSource.TryLoginSilentlyAsync(cancellationToken).ConfigureAwait(false);
    }

    private static IAuthenticationSource? GetDefaultAuthenticationInternal()
    {
        var defaultName = MineOrcApp.AccountManager.DefaultAccount;
        if (string.IsNullOrWhiteSpace(defaultName)
            || !MineOrcApp.AccountManager.TryGetAccount(defaultName, out var account))
        {
            MyOutput.Error(Texts.CommandLaunchFailNoDefaultAccount);
            return null;
        }

        return account.GetAuthenticationSource();
    }

    private static async Task<bool> RestoreInternalAsync(ClientManifest version,
        CancellationToken cancellationToken)
    {
        IEnumerable<IAsyncForegroundAction> actions =
        [
            new RestoreClientJarAction(version.Id),
            new RestoreAssetsAction(version.AssetIndex),
            new RestoreLibrariesAction(version.Libraries),
        ];

        return await ForegroundActions.ExecuteMany(actions, cancellationToken)
            .ConfigureAwait(false);
    }
}