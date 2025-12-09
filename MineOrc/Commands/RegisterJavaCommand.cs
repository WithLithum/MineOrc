// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using System.Text.Json;
using MineOrc.Foundation.Runtime.Java;
using MineOrc.Resources;

namespace MineOrc.Commands;

internal static class RegisterJavaCommand
{
    private static readonly Argument<string> NameArgument = CommandHelper.StringArgument("name",
        Texts.JavaRegisterNameArgument);

    private static readonly Argument<int> VersionArgument = CommandHelper.Int32Argument("version",
        Texts.JavaRegisterVersionArgument);

    private static readonly Argument<FileInfo> ExecutableArgument = CommandHelper.FileArgument(
        "executable",
        Texts.JavaRegisterPathArgument);

    internal static Command CreateCommand()
    {
        var command = new Command("register",
            Texts.JavaRegisterCommand)
        {
            NameArgument,
            VersionArgument,
            ExecutableArgument,
        };

        command.SetAction(ExecuteAsync);
        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse,
        CancellationToken cancellationToken)
    {
        var name = parse.GetRequiredValue(NameArgument);
        var version = parse.GetRequiredValue(VersionArgument);
        var executable = parse.GetRequiredValue(ExecutableArgument);

        if (!MineOrcApp.JavaRegistry.TryAdd(name,
                new JavaInfo(version,
                    executable.FullName)))
        {
            MyOutput.Error(Texts.JavaRegisterFailExistingName, name);
            return 1;
        }

        // Save our changes
        return await JavaCommands.SaveChangesAsync().ConfigureAwait(false);
    }
}