// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Platforms.Fabric.Meta;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Commands;

public static class SearchFabricCommand
{
    private static readonly Argument<string> ArgumentGameVersion = new("gameVersion")
    {
        Description = Texts.CommandFabricSearchArgumentGameVersion,
    };
    
    public static Command CreateCommand()
    {
        var command = new Command("search",
            Texts.CommandFabricSearch)
        {
            ArgumentGameVersion,
            CommonArgs.SearchLimit,
        };
        
        command.SetAction(ExecuteAsync);
        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse, CancellationToken cancellationToken)
    {
        var gameVersion = parse.GetRequiredValue(ArgumentGameVersion);
        var limit = parse.GetValue(CommonArgs.SearchLimit);

        IEnumerable<FabricLoaderMeta> list;
        try
        {
            list = await FabricMetaService
                .GetSupportedLoadersAsync(gameVersion, MineOrcApp.HttpClient)
                .ConfigureAwait(false);
        }
        catch (Exception e) when (e is IOException or HttpRequestException)
        {
            MyOutput.Error(e, Texts.OperationGenericExceptionError);
            return ExitCodes.Failure;
        }

        if (limit != 0)
        {
            list = list.Take(limit);
        }

        foreach (var loader in list)
        {
            AnsiConsole.WriteLine(loader.Loader.Version);
        }
        
        return ExitCodes.Success;
    }
}