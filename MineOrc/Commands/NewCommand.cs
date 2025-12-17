// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using System.CommandLine.Parsing;
using System.Text.Json;
using MineOrc.Foundation.Instancing;
using MineOrc.Instancing;
using MineOrc.Instancing.Operations;
using MineOrc.Management.Profiles;
using MineOrc.Resources;

namespace MineOrc.Commands;

internal static class NewCommand
{
    private static readonly Argument<string> NameArgument = new("name")
    {
        Description = Texts.CommandNewArgumentName,
        Validators = { ValidateName },
    };

    private static readonly Argument<string> VersionArgument = new("version")
    {
        Description = Texts.CommandNewArgumentVersion,
    };

    private static readonly Option<bool> NoInstallVersionOption = new("-N", "--no-install-version")
    {
        Description = Texts.CommandNewOptionNoInstallVersion,
    };

    private static void ValidateName(ArgumentResult arg)
    {
        var value = arg.GetValueOrDefault<string>();
        if (string.IsNullOrWhiteSpace(value)
            || !ProfileManager.ProfileNameRegex.IsMatch(value))
        {
            arg.AddError(Texts.FormatCommandNewFailNameInvalid(
                value,
                ProfileManager.ProfileNameRegex));
        }
    }

    internal static Command CreateCommand()
    {
        var command = new Command("new", Texts.CommandNew)
        {
            NameArgument,
            VersionArgument,
            NoInstallVersionOption,
        };

        command.SetAction(ExecuteAsync);

        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse,
        CancellationToken cancellationToken)
    {
        var name = parse.GetRequiredValue(NameArgument);
        var version = parse.GetRequiredValue(VersionArgument);
        var noInstallVersion = parse.GetValue(NoInstallVersionOption);

        if (MineOrcApp.ProfileManager.HasProfile(name))
        {
            MyOutput.Error(Texts.FormatCommandNewFailAlreadyExists(name));
            return ExitCodes.Failure;
        }

        if (!await ResolveVersionAsync(version,
                noInstallVersion,
                cancellationToken).ConfigureAwait(false))
        {
            return ExitCodes.Failure;
        }

        // Install the profile officially
        try
        {
            await MineOrcApp.ProfileManager.CreateProfileAsync(name,
                new ProfileInfo
                {
                    ClientVersion = version,
                    Created = DateTime.UtcNow,
                }).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is IOException or JsonException)
        {
            MyOutput.Error(ex, Texts.OperationGenericExceptionError);
            return ExitCodes.Failure;
        }

        return ExitCodes.Success;
    }

    private static async ValueTask<bool> ResolveVersionAsync(string version, bool noInstallVersion,
        CancellationToken cancellationToken)
    {
        if (GameApplication.Versions.Exists(version))
        {
            return true;
        }

        if (noInstallVersion)
        {
            MyOutput.Error(Texts.FormatCommandNewFailNoInstallVersion(version));
            return false;
        }

        // Install client.json & restore client.jar
        return await ForegroundActions.ExecuteOne(new InstallClientManifestAction(version),
                       cancellationToken)
                   .ConfigureAwait(false)
               && await ForegroundActions.ExecuteOne(new RestoreClientJarAction(version),
                       cancellationToken)
                   .ConfigureAwait(false);
    }
}