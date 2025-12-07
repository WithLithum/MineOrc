// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Instancing.Operations;

public interface IAsyncForegroundAction
{
    string Name { get; }
    
    Task<bool> ExecuteAsync(CancellationToken cancellationToken);
}