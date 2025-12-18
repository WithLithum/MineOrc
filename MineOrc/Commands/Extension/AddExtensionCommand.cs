// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Frozen;
using System.CommandLine;
using MineOrc.Foundation.Instancing;
using MineOrc.Instancing;
using MineOrc.Resources;
using MineOrc.UI;
using MineOrc.UI.Utilities;

namespace MineOrc.Commands.Extension;

public static class AddExtensionCommand
{
    private static readonly Argument<string> ArgumentType = CommandHelper.StringArgument(
        "type",
        Texts.CommandExtensionAddArgumentName);

    private static readonly Argument<string> ArgumentVersion = CommandHelper.StringArgument(
        "version",
        Texts.CommandExtensionAddArgumentVersion);

    private static readonly Argument<string> ArgumentProfile = CommandHelper.StringArgument(
        "profile",
        Texts.CommandExtensionAddArgumentProfile);

    private static readonly Option<bool> OptionForce = CommandHelper.Switch("--force",
        Texts.CommandExtensionAddOptionForce);
    
    public static Command Create()
    {
        var command = new Command("add", Texts.CommandExtensionAdd)
        {
            ArgumentType,
            ArgumentVersion,
            ArgumentProfile,
            OptionForce,
        };
        
        command.SetAction(ExecuteAsync);
        
        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse, CancellationToken cancellationToken)
    {
        var type = parse.GetRequiredValue(ArgumentType);
        var version = parse.GetRequiredValue(ArgumentVersion);
        var profileName = parse.GetRequiredValue(ArgumentProfile);
        var force = parse.GetValue(OptionForce);
        
        // Get profile
        var profile = await MineOrcApp.ProfileManager.GetProfileOrDefaultAsync(profileName)
            .ConfigureAwait(false);
        if (profile == null)
        {
            CommonMsg.ErrorNoProfile(profileName);
            return ExitCodes.Failure;
        }
        
        // Get metadata
        var metadata = profile.Metadata;
        
        if (!ExtensionService.Providers.TryGetValue(type, out var provider))
        {
            MyOutput.Error(Texts.FormatCommandExtensionFailNoPlatform(type));
            return ExitCodes.Failure;
        }
        
        // Check for --force
        if (!force && metadata.Extensions != null && metadata.Extensions.ContainsKey(type))
        {
            MyOutput.Error(Texts.FormatCommandExtensionAddFailForce(type));
            return ExitCodes.Failure;
        }
        
        // Install
        ProfileExtension? extension = null;
        if (!await PCall.NetworkAsync(async () =>
                extension = await provider.CreateExtensionAsync(version,
                    metadata.ClientVersion,
                    MineOrcApp.HttpClient,
                    cancellationToken).ConfigureAwait(false)).ConfigureAwait(false))
        {
            return ExitCodes.Failure;
        }
        
        // Update profile
        var dict = new Dictionary<string, ProfileExtension>(metadata.Extensions
            ?? FrozenDictionary<string, ProfileExtension>.Empty)
        {
            [type] = extension!,
        };
        var newMeta = metadata with { Extensions = dict };

        return await PCall.LocalIoAsync(() => profile.UpdateMetadataAsync(newMeta))
                .ConfigureAwait(false)
            ? ExitCodes.Success
            : ExitCodes.Failure;
    }
}