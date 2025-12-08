// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Results;

public static class NetworkResultExtensions
{
    extension(NetworkResult self)
    {
        public bool IsOk => self is OkNetworkResult;
    }
}