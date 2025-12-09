// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using System.Globalization;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Commands;

internal static class ListJavaCommand
{
    internal static Command CreateCommand()
    {
        var command = new Command("list",
            Texts.JavaListCommand);
        
        command.SetAction(Execute);
        return command;
    }

    private static void Execute(ParseResult parse)
    {
        var table = new Table();
        table.AddColumns(Texts.JavaRegistryNameColumn,
            Texts.JavaRegisterVersionColumn,
            Texts.JavaRegistryAutoAddedColumn,
            Texts.JavaRegistryPathColumn);
        
        var javas = MineOrcApp.JavaRegistry.Items;

        foreach (var (key, info) in javas)
        {
            table.AddRow(key,
                info.MajorVersion.ToString(CultureInfo.InvariantCulture),
                info.AutoAdded.ToString(CultureInfo.CurrentUICulture),
                info.ExecutablePath);
        }
        
        AnsiConsole.Write(table);
    }
}