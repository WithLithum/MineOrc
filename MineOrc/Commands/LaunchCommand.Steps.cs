// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using MineOrc.Foundation.Instancing;
using MineOrc.Foundation.Runtime;
using MineOrc.Foundation.Runtime.Launch;
using MineOrc.Instancing;
using MineOrc.Resources;
using MineOrc.UI;

namespace MineOrc.Commands;

internal partial class LaunchCommand
{
    [MemberNotNullWhen(true,
        nameof(_profile))]
    private async Task<bool> ExecuteProfileStepAsync()
    {
        _profile = await MineOrcApp.ProfileManager.GetProfileOrDefaultAsync(ProfileName)
            .ConfigureAwait(false);
        if (_profile == null)
        {
            CommonMsg.ErrorNoProfile(ProfileName);
            return false;
        }

        return true;
    }

    [MemberNotNullWhen(true, nameof(_version))]
    private async Task<bool> ExecuteVersionStepAsync(CancellationToken cancellationToken)
    {
        if (!GameApplication.Versions.Exists(_profile!.Metadata.ClientVersion))
        {
            MyOutput.Error(Texts.FormatCommandLaunchFailNoVersion(ProfileName));
            return false;
        }

        _version = await GameApplication.Versions
            .GetManifestAsync(_profile.Metadata.ClientVersion, cancellationToken)
            .ConfigureAwait(false);

        return true;
    }

    [MemberNotNullWhen(true, nameof(_auth))]
    private async Task<bool> ExecuteAuthenticationStepAsync(CancellationToken cancellationToken)
    {
        var auth = await AuthenticateAsync(cancellationToken).ConfigureAwait(false);
        if (auth == null)
        {
            return false;
        }

        _auth = auth;
        return true;
    }

    private async Task<bool> ExecuteRestoreStepAsync(CancellationToken cancellationToken)
    {
        if (!NoRestore)
        {
            if (!await RestoreInternalAsync(_version!, _profile!.Metadata, cancellationToken)
                    .ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }

    [MemberNotNullWhen(true, nameof(_javaCommand))]
    private bool ExecuteJavaStep()
    {
        _javaCommand = UserJavaExecutable ?? GetJava(UserJavaName);
        return _javaCommand != null;
    }

    [MemberNotNullWhen(true, nameof(_nativesDirectory))]
    private bool ExecuteNativeStep()
    {
        _nativesDirectory = Path.Combine(Path.GetTempPath(),
            Path.GetRandomFileName());

        try
        {
            Directory.CreateDirectory(_nativesDirectory);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MyOutput.Error(Texts.OperationGenericMakeDirFail);
            return false;
        }

        return true;
    }

    private async Task ExecuteClassPathStepAsync(CancellationToken cancellationToken)
    {
        var evaluated =
            await LibraryEvaluator.EvaluateAsync(ProfileOrchestrator.GetLibraries(_version!,
                        _profile!.Metadata),
                    cancellationToken)
                .ConfigureAwait(false);

        _classPath = evaluated.Select(x => GameApplication.Libraries.GetArtefactPath(x))
            .Append(GameApplication.Versions.GetJarPath(_version!.Id));
    }

    [MemberNotNull(nameof(_jvmSettings),
        nameof(_gameSettings))]
    private void ExecuteSettingsStep()
    {
        _gameSettings = new LaunchGameSettings
        {
            AssetsRoot = GameApplication.Assets.RootDirectory,
            AssetsVersion = _version!.Assets,
            Session = _auth!,
            ClientId = MineOrcApp.Secrets.EntraAppId,
            GameDirectory = _profile!.Root,
            VersionName = _version.Id,
            VersionType = _version.Type,
            IsDemoMode = Demo,
        };

        _jvmSettings = new LaunchJvmSettings
        {
            LauncherBrand = nameof(MineOrc),
            LauncherVersion = MineOrcApp.Version,
            MainClass = ProfileOrchestrator.GetMainClass(_version, _profile!.Metadata),
            NativesDirectory = _nativesDirectory!,
            MaxMemory = MaxMemory != 0 ? MaxMemory : DefaultMaxMemory,
            MinMemory = MinMemory != 0 ? MinMemory : null,
        };
    }

    private static async Task<int> ExecuteLaunchStepAsync(ProcessStartInfo startInfo,
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
}