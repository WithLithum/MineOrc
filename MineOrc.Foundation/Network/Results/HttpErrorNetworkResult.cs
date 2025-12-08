// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using JetBrains.Annotations;

namespace MineOrc.Foundation.Network.Results;

[PublicAPI]
public sealed record HttpErrorNetworkResult : NetworkResult
{
    internal HttpErrorNetworkResult(string message, HttpRequestError code)
    {
        Message = message;
        Code = code;
    }
    
    public string Message { get; }
    
    public HttpRequestError Code { get; }

    public override string ToString()
    {
        return $"{Code}: {Message}";
    }
}