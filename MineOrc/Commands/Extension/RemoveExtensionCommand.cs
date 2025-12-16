// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Frozen;
using System.CommandLine;
using MineOrc.Foundation.Instancing;
using MineOrc.Resources;
using MineOrc.UI.Utilities;

namespace MineOrc.Commands.Extension;

public static class RemoveExtensionCommand
{
    private static readonly Argument<string> ArgumentName = CommandHelper.StringArgument("name",
        Texts.CommandExtensionRemoveArgumentName);

    private static readonly Argument<string> ArgumentProfile = CommandHelper.StringArgument(
        "profile",
        Texts.CommandExtensionRemoveArgumentProfile);

    public static Command Create()
    {
        var command = new Command("remove",
            Texts.CommandExtensionRemove)
        {
            ArgumentName,
            ArgumentProfile,
        };

        command.SetAction(ExecuteAsync);

        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse,
        CancellationToken cancellationToken)
    {
        // Arguments
        var extensionName = parse.GetRequiredValue(ArgumentName);
        var profileName = parse.GetRequiredValue(ArgumentProfile);

        // Get profile
        if (!MineOrcApp.ProfileManager.HasProfile(profileName))
        {
            MyOutput.Error(Texts.CommandGenericNoProfile, profileName);
            return ExitCodes.Failure;
        }

        var profile = await MineOrcApp.ProfileManager.ReadProfileAsync(profileName)
            .ConfigureAwait(false);

        // Check existence
        if (profile.Extensions == null
            || !profile.Extensions.ContainsKey(extensionName))
        {
            MyOutput.Error(Texts.CommandExtensionRemoveFailNoExtension);
            return ExitCodes.Failure;
        }

        // Update profile
        var dict = new Dictionary<string, ProfileExtension>(profile.Extensions
                                                            ?? FrozenDictionary<string,
                                                                ProfileExtension>.Empty);
        dict.Remove(extensionName);

        return await PCall.LocalIoAsync(() => MineOrcApp.ProfileManager.UpdateProfileAsync(profileName,
            profile with { Extensions = dict })).ConfigureAwait(false)
            ? ExitCodes.Success
            : ExitCodes.Failure;
    }
}