// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MineOrc.Resources;
using Spectre.Console;

namespace MineOrc;

public static class MyOutput
{
    public static void Error(string message)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold white]{MineOrcApp.BaseName}:[/] [red]{message}[/]");
    }

    [StringFormatMethod("format")]
    public static void Error([StringSyntax("CompositeFormat")] string format, params object[] args)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold white]{MineOrcApp.BaseName}:[/] {string.Format(format, args)}");
    }

    public static void Error(Exception exception, string message)
    {
        Error(message);
        AnsiConsole.WriteException(exception, ExceptionFormats.ShortenEverything);
    }
    
    public static void DownloadError(HttpRequestException exception)
    {
        Error(Texts.OperationDownloadFailHttp, exception.StatusCode?.ToString("D")
                                               ?? exception.HttpRequestError.ToString("G"));
    }
}