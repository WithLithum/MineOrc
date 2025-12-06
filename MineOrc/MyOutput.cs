// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Spectre.Console;

namespace MineOrc;

public static class MyOutput
{
    public static void Error(string message)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold white]{MineOrcApp.BaseName}:[/] [red]{message}[/]");
    }

    public static void Error(Exception exception, string message)
    {
        Error(message);
        AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
    }
}