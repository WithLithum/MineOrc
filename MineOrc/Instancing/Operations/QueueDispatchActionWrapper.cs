// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Utilities;
using MineOrc.UI;
using Spectre.Console;

namespace MineOrc.Instancing.Operations;

public abstract class QueueDispatchActionWrapper<T> : IAsyncForegroundAction
{
    protected QueueDispatchActionWrapper(string name)
    {
        Name = name;
    }

    protected abstract ValueTask<QueueDispatchAction<T>> CreateActionAsync(IProgress<double> progress);

    public string Name { get; }

    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        var progress = AnsiConsole.Progress();
        var success = true;
        await progress.StartAsync(async c =>
        {
            var task = c.AddTask(Name);
            var prog = new ProgressAction(task);

            var action = await CreateActionAsync(prog).ConfigureAwait(false);
            success = await action.DoAsync(cancellationToken).ConfigureAwait(false);
        }).ConfigureAwait(false);

        return success;
    }
}