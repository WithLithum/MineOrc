// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using JetBrains.Annotations;

namespace MineOrc.Foundation.Network.Results;

[PublicAPI]
public sealed record IoErrorNetworkResult : NetworkResult
{
    public IoErrorNetworkResult(string message)
    {
        Message = message;
    }
    
    public string Message { get; }
    
    public override string ToString() => Message;
}