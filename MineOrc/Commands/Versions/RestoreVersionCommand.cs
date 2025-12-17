// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using System.Text.Json;
using MineOrc.Foundation.Manifest;
using MineOrc.Instancing;
using MineOrc.Instancing.Operations;
using MineOrc.Resources;

namespace MineOrc.Commands.Versions;

public static class RestoreVersionCommand
{
    private static readonly Argument<string> VersionArgument = new("version")
    {
        Description = Texts.VersionRestoreVersionArgument,
    };

    internal static Command CreateCommand()
    {
        var command = new Command("restore", Texts.VersionRestoreCommand)
        {
            VersionArgument,
        };

        command.SetAction(ExecuteAsync);
        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parseResult,
        CancellationToken cancellationToken)
    {
        var version = parseResult.GetRequiredValue(VersionArgument);

        if (!GameApplication.Versions.Exists(version))
        {
            MyOutput.Error(Texts.FormatCommandVersionRestoreFailNoVersion(version));
            return 1;
        }

        ClientManifest manifest;
        try
        {
            manifest = await GameApplication.Versions.GetManifestAsync(version,
                    cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is JsonException or InvalidOperationException or IOException)
        {
            MyOutput.Error(ex, Texts.OperationReadClientManifestFailed);
            return 1;
        }

        IEnumerable<IAsyncForegroundAction> actions =
        [
            new RestoreClientJarAction(version),
            new RestoreAssetsAction(manifest.AssetIndex),
            new RestoreLibrariesAction(manifest.Libraries),
        ];

        return await ForegroundActions.ExecuteMany(actions, cancellationToken)
            .ConfigureAwait(false)
            ? 0
            : 1;
    }
}