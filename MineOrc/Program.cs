// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc;
using MineOrc.Commands;
using MineOrc.Resources;
using Command = System.CommandLine.Command;

var command = new RootCommand(Texts.RootDescription)
{
    new Command("version", Texts.VersionBranch)
    {
        SearchVersionCommand.CreateCommand(),
        InstallVersionCommand.CreateCommand(),
        RestoreVersionCommand.CreateCommand(),
    },
    new Command("java", Texts.JavaBranch)
    {
        ListJavaCommand.CreateCommand(),
        RegisterJavaCommand.CreateCommand(),
        DefaultJavaCommand.CreateCommand(),
    },
    NewCommand.CreateCommand(),
    LaunchCommand.CreateCommand(),
    LoginCommand.CreateCommand(),
};

AppDomain.CurrentDomain.UnhandledException += (_, args) =>
{
    MyOutput.Error((Exception)args.ExceptionObject, "unhandled exception");
    MyOutput.Error(((Exception)args.ExceptionObject).Message);
};

if (!await MineOrcApp.InitializeAsync().ConfigureAwait(false))
{
    return ExitCodes.InitializationFailed;
}

try
{
    return await command.Parse(args).InvokeAsync(
        new InvocationConfiguration
        {
            EnableDefaultExceptionHandler = false,
        }).ConfigureAwait(false);
}
catch (OperationCanceledException)
{
    // Operation cancels does not necessarily mean something is wrong.
    return 0;
}