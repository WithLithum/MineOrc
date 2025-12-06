// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using System.Diagnostics;
using MineOrc;
using MineOrc.Commands;
using MineOrc.Resources;
using Command = System.CommandLine.Command;

var command = new RootCommand(Texts.RootDescription)
{
    new Command("version", Texts.VersionBranch)
    {
        SearchVersionCommand.CreateCommand()
    }
};

#if DEBUG
try
{
#endif
    return await command.Parse(args).InvokeAsync();
#if DEBUG
}
catch (Exception ex)
{
    Debugger.Break();
    MyOutput.Error(ex, "unhandled exception");
    return 1;
}
#endif