// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Instancing;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Commands;

public static class ListExtensionCommand
{
    public static Command Create()
    {
        var command = new Command("list", Texts.CommandExtensionList);

        command.SetAction(Execute);

        return command;
    }

    private static void Execute(ParseResult obj)
    {
        foreach (var extension in PlatformService.ExtensionProviders.Keys)
        {
            AnsiConsole.WriteLine(extension);
        }
    }
}