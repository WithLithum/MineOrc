// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc.Commands.Runtimes;

internal static class DefaultJavaCommand
{
    private static readonly Argument<string?> ValueArgument = new("value")
    {
        Description = Texts.JavaDefaultValueArgument,
        DefaultValueFactory = _ => null,
    };

    internal static Command CreateCommand()
    {
        var command = new Command("default",
            Texts.JavaDefaultCommand)
        {
            ValueArgument,
        };
        
        command.SetAction(ExecuteAsync);
        
        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse,
        CancellationToken cancellationToken)
    {
        var value = parse.GetValue(ValueArgument);
        if (value == null)
        {
            return ExecuteDisplay();
        }
        else
        {
            return await ExecuteSetAsync(value).ConfigureAwait(false);
        }
    }

    private static async ValueTask<int> ExecuteSetAsync(string value)
    {
        if (!MineOrcApp.JavaRegistry.Items.ContainsKey(value))
        {
            MyOutput.Error(Texts.JavaFailNoSuchRuntime, value);
            return ExitCodes.Failure;
        }
        
        MineOrcApp.JavaRegistry.DefaultKey = value;

        return await JavaCommands.SaveChangesAsync().ConfigureAwait(false);
    }

    private static int ExecuteDisplay()
    {
        var defaultValue = MineOrcApp.JavaRegistry.GetValidDefault();
        if (defaultValue == null)
        {
            MyOutput.Error(Texts.JavaDefaultFailNoDefault);
            return ExitCodes.AbsenceOfValue;
        }
        
        AnsiConsole.WriteLine(defaultValue);
        return ExitCodes.Success;
    }
}