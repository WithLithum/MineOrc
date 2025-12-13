// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Security;

public interface IAuthenticationSource
{
    /// <summary>
    /// Attempts to log in without requiring user input.
    /// </summary>
    /// <returns>The authentication result.</returns>
    Task<MinecraftAuthResult> TryLoginSilentlyAsync(CancellationToken cancellationToken);
}