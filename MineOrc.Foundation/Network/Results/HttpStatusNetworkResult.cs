// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net;
using JetBrains.Annotations;

namespace MineOrc.Foundation.Network.Results;

[PublicAPI]
public sealed record HttpStatusNetworkResult : NetworkResult
{
    internal HttpStatusNetworkResult(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }
    
    public HttpStatusCode StatusCode { get; }

    public override string ToString()
    {
        return $"HTTP {StatusCode:D}";
    }
}