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
        await MineOrcApp.SecureAuthority.SignInWithDeviceCodeAsync(async code =>
            {
                AnsiConsole.WriteLine("Please login with the device code {0} at the following URL:", code.DeviceCode);
                AnsiConsole.WriteLine(code.VerificationUrl);
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine(code.Message);
            },
            cancellationToken)
            .ConfigureAwait(false);
    }
}