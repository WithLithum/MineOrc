// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
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

    private static async Task ExecuteAsync(ParseResult parse, CancellationToken cancellationToken)
    {
        var msaResult = await MineOrcApp.SecureAuthority.SignInWithDeviceCodeAsync(code =>
            {
                AnsiConsole.MarkupLine("[bold underline yellow]Device Code Login Flow[/]");
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[white]Please open the following URL in your browser:[/]");
                AnsiConsole.MarkupLineInterpolated($"[aqua underline]{code.VerificationUrl}[/]");
                AnsiConsole.MarkupLine("[white]And enter the following code:[/]");
                AnsiConsole.MarkupLineInterpolated($"[lime]{code.UserCode}[/]");
                AnsiConsole.WriteLine();
                
                return Task.CompletedTask;
            },
            cancellationToken)
            .ConfigureAwait(false);
        
        // TODO xbox login
    }
}