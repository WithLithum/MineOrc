// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Instancing;
using MineOrc.Instancing.Operations;
using MineOrc.Resources;

namespace MineOrc.Commands;

internal static class InstallVersionCommand
{
    private static readonly Argument<string> VersionIdArgument = new("id")
    {
        Description = Texts.VersionInstallIdArgument
    };

    private static readonly Option<bool> NoRestoreOption = new("--no-restore")
    {
        Description = Texts.VersionInstallNoRestoreOption
    };

    public static Command CreateCommand()
    {
        var command = new Command("install",
            Texts.VersionInstallCommand)
        {
            VersionIdArgument,
            NoRestoreOption
        };
        
        command.SetAction(ExecuteAsync);
        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parseResult,
        CancellationToken cancellationToken)
    {
        var id = parseResult.GetRequiredValue(VersionIdArgument);
        var noRestore = parseResult.GetValue(NoRestoreOption);

        if (GameApplication.Versions.Exists(id))
        {
            MyOutput.Error(Texts.VersionInstallCommandFailExists);
            return 1;
        }
        
        if (!await ForegroundActions.ExecuteOne(new InstallClientManifestAction(id),
                cancellationToken)
                .ConfigureAwait(false))
        {
            return 1;
        }

        if (noRestore)
        {
            return 0;
        }

        return await ForegroundActions.ExecuteOne(new RestoreClientJarAction(id),
                cancellationToken)
            .ConfigureAwait(false)
            ? 0
            : 1;
    }
}