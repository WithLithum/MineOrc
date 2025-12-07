// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Resources;
using SmartFormat;
using Spectre.Console;

namespace MineOrc.Instancing.Operations;

public static class ForegroundActions
{
    public static async Task<bool> ExecuteOne(IAsyncForegroundAction action,
        CancellationToken cancellationToken = default)
    {
        var actionName = action.Name;
        AnsiConsole.MarkupLineInterpolated($"[bold cyan]-> [/][white]{actionName}[/]");

        try
        {
            return await action.ExecuteAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            MyOutput.Error(ex, Smart.Format(Texts.ActionExecuteError,
                new { ActionName = actionName }));
            return false;
        }
    }

    public static async Task<bool> ExecuteMany(IEnumerable<IAsyncForegroundAction> actions,
        CancellationToken cancellationToken = default)
    {
        foreach (var action in actions)
        {
            if (!await ExecuteOne(action, cancellationToken).ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }
}