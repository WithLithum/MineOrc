// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.CommandLine;
using MineOrc.Management.Runtime.Java;
using MineOrc.Resources;
using SmartFormat;

namespace MineOrc.Commands.Runtimes;

public sealed class ScanJavaCommand
{
    public static Command Create()
    {
        var command = new Command("scan",
            Texts.CommandJavaScan);

        command.SetAction(ExecuteAsync);

        return command;
    }

    private static async Task<int> ExecuteAsync(ParseResult parse,
        CancellationToken cancellationToken)
    {
        var locators = new List<IJavaLocator>();
        if (OperatingSystem.IsWindows())
        {
            locators.Add(new JavaSoftJavaLocator());
        }

        if (locators.Count == 0)
        {
            MyOutput.Error(Texts.CommandJavaScanFailNoLocator);
            return ExitCodes.Failure;
        }

        var count = 0;
        await Task.Run(() =>
            {
                MineOrcApp.JavaRegistry.RemoveAutoAdded();
                foreach (var javas in
                         locators.Select(locator => locator.SearchJava()))
                {
                    MineOrcApp.JavaRegistry.AddRange(javas);
                    count++;
                }
            },
            cancellationToken).ConfigureAwait(false);

        MyOutput.Notice(Smart.Format(Texts.CommandJavaScanResult,
            new { amount = count }));
        return await JavaCommands.SaveChangesAsync().ConfigureAwait(false);
    }
}