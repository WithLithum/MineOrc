// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Utilities;

public abstract class QueueDispatchAction<T>
{
    private int _failCount;
    private int _completeCount;
    private readonly SemaphoreSlim _semaphore;
    private readonly IReadOnlyCollection<T> _payloads;

    protected QueueDispatchAction(IReadOnlyCollection<T> payloads,
        int semaphoreCount = 3)
    {
        _payloads = payloads;
        _semaphore = new SemaphoreSlim(semaphoreCount, semaphoreCount);
    }
    
    public async Task<bool> DoAsync(IStatusReporter? progress = null,
        CancellationToken cancellationToken = default)
    {
        _failCount = 0;
        
        await Task.WhenAll(PushWorksAsync(progress, cancellationToken)).ConfigureAwait(false);
        
        return _failCount == 0;
    }

    private IEnumerable<Task> PushWorksAsync(IStatusReporter? progress,
        CancellationToken cancellationToken)
    {
        foreach (var payload in _payloads)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return DispatchWorkAsync(payload, progress, cancellationToken);
        }
    }
    
    private async Task<bool> DispatchWorkAsync(T payload,
        IStatusReporter? progress,
        CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        var succeeded = await ExecuteActionAsync(payload, cancellationToken)
            .ConfigureAwait(false);

        _semaphore.Release();
                    
        // Communication success
        Interlocked.Increment(ref _completeCount);
        if (!succeeded)
        {
            Interlocked.Increment(ref _failCount);
        }
                    
        // Report current progress.
        progress?.SetText($"{_completeCount}/{_payloads.Count}");
        
        return succeeded;
    }
    
    protected abstract Task<bool> ExecuteActionAsync(T payload, CancellationToken cancellationToken);
}