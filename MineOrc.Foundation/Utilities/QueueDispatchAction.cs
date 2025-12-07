// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Utilities;

public abstract class QueueDispatchAction<T>
{
    protected int completedCount;
    
    private int _failCount;
    private readonly SemaphoreSlim _semaphore;
    private readonly IReadOnlyCollection<T> _payloads;

    protected QueueDispatchAction(IReadOnlyCollection<T> payloads,
        int semaphoreCount = 3)
    {
        _payloads = payloads;
        _semaphore = new  SemaphoreSlim(semaphoreCount, semaphoreCount);
    }

    public int CompletedCount => completedCount;
    
    public async Task<bool> DoAsync(CancellationToken cancellationToken = default)
    {
        _failCount = 0;
        completedCount = 0;
        
        await Task.WhenAll(PushWorksAsync(cancellationToken));
        
        return _failCount == 0;
    }

    private IEnumerable<Task> PushWorksAsync(CancellationToken cancellationToken = default)
    {
        foreach (var payload in _payloads)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return DispatchWorkAsync(payload, cancellationToken);
        }
    }
    
    private async Task<bool> DispatchWorkAsync(T payload, CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken)
            .ConfigureAwait(false);

        bool succeeded;

        try
        {
            succeeded = await ExecuteActionAsync(payload, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            ReportException(ex);
            succeeded = false;
        }

        _semaphore.Release();
                    
        // Communication success
        Interlocked.Increment(ref completedCount);
        if (!succeeded)
        {
            Interlocked.Increment(ref _failCount);
        }
                    
        // Report current progress.
        var currentProgress = (double)completedCount / _payloads.Count * 100;
        ReportProgress(currentProgress);

        return succeeded;
    }
    
    protected abstract Task<bool> ExecuteActionAsync(T payload, CancellationToken cancellationToken);
    
    protected virtual void ReportException(Exception exception)
    {
    }
    
    protected virtual void ReportProgress(double progress)
    {
    }
}