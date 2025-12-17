// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Network.Security;
using Spectre.Console;

namespace MineOrc.Commands;

public static class LoginCommand
{
    internal static Command CreateCommand()
    {
        var command = new Command("login");

        command.SetAction(ExecuteAsync);
        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse,
        CancellationToken cancellationToken)
    {
        var result = await InteractiveAuthenticator.AuthenticateAsync(cancellationToken)
            .ConfigureAwait(false);

        if (result == null)
        {
            return ExitCodes.Failure;
        }

        AnsiConsole.MarkupLineInterpolated(
            $"[green]Welcome back,[/] [yellow]{result.Profile.Name}[/][green]![/]");
        return ExitCodes.Success;
    }
}