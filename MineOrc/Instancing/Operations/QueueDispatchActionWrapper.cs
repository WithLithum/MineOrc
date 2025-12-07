// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Utilities;
using MineOrc.UI;
using Spectre.Console;

namespace MineOrc.Instancing.Operations;

public sealed class QueueDispatchActionWrapper<T> : IAsyncForegroundAction
{
    private readonly Func<IProgressEx<double>, QueueDispatchAction<T>> _actionFactory;
    
    public QueueDispatchActionWrapper(string name, Func<IProgressEx<double>, QueueDispatchAction<T>> actionFactory)
    {
        Name = name;
        _actionFactory = actionFactory;
    }

    public string Name { get; }

    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        var progress = AnsiConsole.Progress();
        var success = true;
        await progress.StartAsync(async c =>
        {
            var task = c.AddTask(Name);
            var prog = new ProgressAction(task);

            var action = _actionFactory(prog);
            success = await action.DoAsync(cancellationToken).ConfigureAwait(false);
        }).ConfigureAwait(false);

        return success;
    }
}