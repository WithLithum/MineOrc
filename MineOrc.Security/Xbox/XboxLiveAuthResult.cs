// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Security.Xbox;

public sealed record XboxLiveAuthResult
{
    public required DateTime IssuedAt { get; init; }
    public required DateTime ExpiresAt { get; init; }
    public required string Token { get; init; }
    public required string UserHash { get; init; }
}