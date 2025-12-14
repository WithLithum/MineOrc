// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Spectre.Console;

namespace MineOrc;

public static class MyOutput
{
    public static void Warn(string message)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold white]{MineOrcApp.BaseName}:[/] [bold yellow]warn:[/] [gold1]{message}[/]");
    }
    
    public static void Warn(Exception exception, string message)
    {
        Warn(message);
        AnsiConsole.MarkupLineInterpolated($"[grey]{exception}[/]");
    }
    
    [StringFormatMethod("format")]
    public static void Warn([StringSyntax("CompositeFormat")] string format,
        params object[] args)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold white]{MineOrcApp.BaseName}:[/] [bold yellow]warn:[/] [gold1]{string.Format(format, args)}[/]");
    }
    
    [StringFormatMethod("format")]
    public static void Warn(Exception exception,
        [StringSyntax("CompositeFormat")] string format,
        params object[] args)
    {
        Warn(format, args);
        AnsiConsole.MarkupLineInterpolated($"[grey]{exception}[/]");
    }
    
    public static void Error(string message)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold white]{MineOrcApp.BaseName}:[/] [red]{message}[/]");
    }

    [StringFormatMethod("format")]
    public static void Error([StringSyntax("CompositeFormat")] string format, params object[] args)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold white]{MineOrcApp.BaseName}:[/] [red]{string.Format(format, args)}[/]");
    }

    public static void Error(Exception exception, string message)
    {
        Error(message);
        AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
    }
    
    [StringFormatMethod("format")]
    public static void Error(Exception exception,
        [StringSyntax("CompositeFormat")] string format,
        params object[] args)
    {
        Error(format, args);
        AnsiConsole.MarkupLineInterpolated($"[grey]{exception}[/]");
    }
}