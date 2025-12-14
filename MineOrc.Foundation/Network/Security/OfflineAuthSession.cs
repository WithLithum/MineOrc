// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Security;

public sealed class OfflineAuthSession : IAuthSession
{
    public string AccessToken => "access_token";

    public DateTimeOffset ExpiresIn => DateTimeOffset.MaxValue;
}