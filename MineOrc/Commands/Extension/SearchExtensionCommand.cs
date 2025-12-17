// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Instancing;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Commands.Extension;

public static class SearchExtensionCommand
{
    private static readonly Argument<string> ArgumentExtensionName = CommandHelper.StringArgument(
        "platform",
        Texts.CommandExtensionArgumentPlatformId);

    private static readonly Argument<string> ArgumentGameVersion = CommandHelper.StringArgument(
        "gameVersion",
        Texts.CommandExtensionArgumentGameVersion);
    
    public static Command Create()
    {
        var command = new Command("search",
            Texts.CommandExtensionSearch)
        {
            ArgumentExtensionName,
            ArgumentGameVersion,
            CommonArgs.SearchLimit,
        };
        
        command.SetAction(ExecuteAsync);
        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse, CancellationToken cancellationToken)
    {
        var platformName = parse.GetRequiredValue(ArgumentExtensionName);
        var gameVersion = parse.GetRequiredValue(ArgumentGameVersion);
        var limit = parse.GetValue(CommonArgs.SearchLimit);

        if (!ExtensionService.Providers.TryGetValue(platformName, out var provider))
        {
            MyOutput.Error(Texts.FormatCommandExtensionFailNoPlatform(platformName));
            return ExitCodes.Failure;
        }

        IEnumerable<string> items;
        try
        {
            items = await provider.GetVersionsAsync(gameVersion,
                MineOrcApp.HttpClient,
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is HttpRequestException or IOException)
        {
            MyOutput.Error(ex, Texts.OperationGenericExceptionError);
            return ExitCodes.Failure;
        }

        if (limit != 0)
        {
            items = items.Take(limit);
        }
        
        foreach (var item in items)
        {
            AnsiConsole.WriteLine(item);
        }

        return ExitCodes.Success;
    }
}