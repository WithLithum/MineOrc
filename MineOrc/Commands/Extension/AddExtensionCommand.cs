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

        // Get data and services
        if (!MineOrcApp.ProfileManager.HasProfile(profileName))
        {
            CommonMsg.ErrorNoProfile(profileName);
            return ExitCodes.Failure;
        }

        var profile = await MineOrcApp.ProfileManager.ReadProfileAsync(profileName)
            .ConfigureAwait(false);
        
        if (!ExtensionService.Providers.TryGetValue(type, out var provider))
        {
            MyOutput.Error(Texts.FormatCommandExtensionFailNoPlatform(type));
            return ExitCodes.Failure;
        }
        
        // Check for --force
        if (!force && profile.Extensions != null && profile.Extensions.ContainsKey(type))
        {
            MyOutput.Error(Texts.FormatCommandExtensionAddFailForce(type));
            return ExitCodes.Failure;
        }
        
        // Install
        ProfileExtension? extension = null;
        var pProfile = profile;
        if (!await PCall.NetworkAsync(async () =>
                extension = await provider.CreateExtensionAsync(version,
                    pProfile.ClientVersion,
                    MineOrcApp.HttpClient,
                    cancellationToken).ConfigureAwait(false)).ConfigureAwait(false))
        {
            return ExitCodes.Failure;
        }
        
        // Update profile
        var dict = new Dictionary<string, ProfileExtension>(profile.Extensions
            ?? FrozenDictionary<string, ProfileExtension>.Empty)
        {
            [type] = extension!,
        };
        profile = profile with { Extensions = dict };

        return await PCall.LocalIoAsync(() => MineOrcApp.ProfileManager.UpdateProfileAsync(profileName, profile))
                .ConfigureAwait(false)
            ? ExitCodes.Success
            : ExitCodes.Failure;
    }
}