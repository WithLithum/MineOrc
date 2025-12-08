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

    protected abstract ValueTask<IReadOnlyCollection<T>?> GetPayloadsAsync(CancellationToken cancellationToken);
    protected abstract QueueDispatchAction<T> CreateAction(IReadOnlyCollection<T> payload);

    public string Name { get; }

    public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
    {
        var payloads = await GetPayloadsAsync(cancellationToken).ConfigureAwait(false);
        if (payloads == null)
        {
            return false;
        }
        
        var success = true;
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .StartAsync($":{Name}", async c =>
        {
            var prog = new SpectreStatusReporter(c, $":{Name} - ");

            var action = CreateAction(payloads);
            success = await action.DoAsync(prog, cancellationToken).ConfigureAwait(false);
        }).ConfigureAwait(false);

        return success;
    }
}