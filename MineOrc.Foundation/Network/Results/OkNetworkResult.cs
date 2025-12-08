// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Results;

internal sealed record OkNetworkResult : NetworkResult
{
    internal static readonly OkNetworkResult Instance = new();

    private OkNetworkResult()
    {
    }
}