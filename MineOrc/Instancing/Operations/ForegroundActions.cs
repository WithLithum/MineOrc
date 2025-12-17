// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Spectre.Console;

namespace MineOrc.Instancing.Operations;

public static class ForegroundActions
{
    public static async Task<bool> ExecuteOne(IAsyncForegroundAction action,
        CancellationToken cancellationToken = default)
    {
        var actionName = action.Name;
        AnsiConsole.MarkupLineInterpolated($"[bold cyan]-> [/][white]{actionName}[/]");

        return await action.ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }

    public static async Task<bool> ExecuteMany(IEnumerable<IAsyncForegroundAction> actions,
        CancellationToken cancellationToken = default)
    {
        foreach (var action in actions)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (!await ExecuteOne(action, cancellationToken).ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }
}