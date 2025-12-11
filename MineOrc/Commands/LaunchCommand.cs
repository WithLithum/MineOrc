// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using System.Diagnostics;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Network.Security;
using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Runtime.Arguments;
using MineOrc.Foundation.Runtime.Launch;
using MineOrc.Instancing;
using MineOrc.Instancing.Operations;
using MineOrc.Resources;

namespace MineOrc.Commands;

internal static class LaunchCommand
{
    private const int DefaultMaxMemory = 2048;
    
    private static readonly Argument<string> ArgumentName = CommandHelper.StringArgument("name",
        Texts.CommandLaunchArgumentName);

    private static readonly Option<bool> OptionNoRestore = CommandHelper.Switch("-N",
        "--no-restore",
        Texts.CommandLaunchOptionNoRestore);

    private static readonly Option<bool> OptionDemo = CommandHelper.Switch("-D",
        "--demo",
        Texts.CommandLaunchOptionDemo);

    private static readonly Option<int> OptionMinMemory = new("--min-memory")
    {
        Description = Texts.CommandLaunchOptionMinMemory,
    };

    private static readonly Option<int> OptionMaxMemory = new("--max-memory")
    {
        Description = Texts.CommandLaunchOptionMaxMemory,
    };

    private static readonly Option<string?> OptionJavaName = new("-j", "--java")
    {
        Description = Texts.CommandLaunchOptionJavaName,
    };

    private static readonly Option<string?> OptionJavaExecutable = new("-J",
        "--java-file")
    {
        Description = Texts.CommandLaunchOptionJavaFile,
    };

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
        };

        command.SetAction(ExecuteAsync);

        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse,
        CancellationToken cancellationToken)
    {
        var profileName = parse.GetRequiredValue(ArgumentName);
        var noRestore = parse.GetValue(OptionNoRestore);
        var demo = parse.GetValue(OptionDemo);
        var javaName = parse.GetValue(OptionJavaName);
        var javaExecutable = parse.GetValue(OptionJavaExecutable);
        var minMemory = parse.GetValue(OptionMinMemory);
        var maxMemory = parse.GetValue(OptionMaxMemory);

        if (!MineOrcApp.ProfileManager.HasProfile(profileName))
        {
            MyOutput.Error(Texts.CommandGenericNoProfile, profileName);
            return ExitCodes.Failure;
        }

        var profile = await MineOrcApp.ProfileManager.ReadProfileAsync(profileName)
            .ConfigureAwait(false);
        var profileDirectory = MineOrcApp.ProfileManager.GetProfileDirectory(profileName);

        // Find the version
        if (!GameApplication.Versions.Exists(profile.ClientVersion))
        {
            MyOutput.Error(Texts.CommandLaunchFailNoVersion, profileName);
            return ExitCodes.Failure;
        }

        var version = await GameApplication.Versions
            .GetManifestAsync(profile.ClientVersion, cancellationToken)
            .ConfigureAwait(false);

        // Restore the version
        if (!await ExecuteStepRestoreAsync(version, noRestore, cancellationToken)
                .ConfigureAwait(false))
        {
            return ExitCodes.Failure;
        }

        // Login
        var auth = await AuthenticateAsync(demo).ConfigureAwait(false);
        if (auth == null)
        {
            return ExitCodes.Failure;
        }

        // Determine the default Java to use 
        javaExecutable ??= GetJava(javaName);
        if (javaExecutable == null)
        {
            return ExitCodes.Failure;
        }

        // Libraries
        var evaluated =
            await LibraryEvaluator.EvaluateAsync(version.Libraries).ConfigureAwait(false);
        var classPath = evaluated.Select(x => GameApplication.Libraries.GetArtefactPath(x))
            .Append(GameApplication.Versions.GetJarPath(version.Id));

        // Natives
        // TODO extraction
        var nativesDirectory = Path.Combine(Path.GetTempPath(),
            Path.GetRandomFileName());
        try
        {
            Directory.CreateDirectory(nativesDirectory);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MyOutput.Error(Texts.OperationGenericMakeDirFail);
            return ExitCodes.Failure;
        }

        // Create the arguments
        var gameSettings = new LaunchGameSettings
        {
            AssetsRoot = GameApplication.Assets.RootDirectory,
            AssetsVersion = version.Assets,
            AuthenticationResult = auth,
            ClientId = "TODO", // TODO make us a real client ID
            GameDirectory = profileDirectory,
            VersionName = version.Id,
            VersionType = version.Type,
            IsDemoMode = demo,
        };
        var jvmSettings = new LaunchJvmSettings
        {
            LauncherBrand = nameof(MineOrc),
            LauncherVersion = MineOrcApp.Version,
            MainClass = version.MainClass,
            NativesDirectory = nativesDirectory,
            MaxMemory = maxMemory != 0 ? maxMemory : DefaultMaxMemory,
            MinMemory = minMemory != 0 ? minMemory : null,
        };

        // Start!
        var startInfo = ArgumentAssembler.CreateStartInfo(javaExecutable,
            version.Arguments,
            classPath,
            jvmSettings,
            gameSettings
        );

        return await RunProcessStepAsync(startInfo, cancellationToken).ConfigureAwait(false);
    }

    private static async Task<bool> ExecuteStepRestoreAsync(ClientManifest version, bool noRestore,
        CancellationToken cancellationToken)
    {
        if (!noRestore)
        {
            if (!await RestoreInternalAsync(version, cancellationToken)
                    .ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }
    
    private static async Task<int> RunProcessStepAsync(ProcessStartInfo startInfo,
        CancellationToken cancellationToken)
    {
        var process = Process.Start(startInfo);
        if (process == null)
        {
            MyOutput.Error(Texts.CommandLaunchFailProcessStart);
            return ExitCodes.Failure;
        }

        await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        return process.ExitCode;
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

    private static async Task<AuthenticationResult?> AuthenticateAsync(bool demo)
    {
        AuthenticationResult? auth;
        if (demo)
        {
            auth = await new DemoAuthenticationSource().TryLoginSilentlyAsync()
                .ConfigureAwait(false);
        }
        else
        {
            auth = await AuthenticateOnlineInternalAsync().ConfigureAwait(false);
            if (auth is not { Succeeded: true })
            {
                return auth;
            }
        }

        return auth;
    }

    private static async Task<AuthenticationResult?> AuthenticateOnlineInternalAsync()
    {
        var defaultName = MineOrcApp.AccountManager.DefaultAccount;
        if (string.IsNullOrWhiteSpace(defaultName)
            || !MineOrcApp.AccountManager.TryGetAccount(defaultName, out var account))
        {
            MyOutput.Error(Texts.CommandLaunchFailNoDefaultAccount);
            return null;
        }

        var result = await account.TryLoginSilentlyAsync().ConfigureAwait(false);
        if (!result.Succeeded)
        {
            MyOutput.Error(Texts.CommandGenericLoginFailed);
            return null;
        }

        return result;
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