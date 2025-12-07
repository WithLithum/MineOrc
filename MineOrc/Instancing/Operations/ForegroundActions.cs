// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Spectre.Console;

namespace MineOrc.Instancing.Operations;

public static class ForegroundActions
{
    public static async Task<bool> ExecuteOne(IAsyncForegroundAction action,
        CancellationToken cancellationToken = default)
    {
        AnsiConsole.MarkupLineInterpolated($"[bold black on white]-> [/][grey23 on white]{action.Name}[/]");

        try
        {
            return await action.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            MyOutput.Error(ex, $"error while executing action {action.Name}");
            return false;
        }
    }
}