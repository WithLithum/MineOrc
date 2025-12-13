// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Security;

public sealed class DemoAuthenticationSource : IAuthenticationSource
{
    public Task<AuthenticationResult> TryLoginSilentlyAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(new AuthenticationResult
        {
            Succeeded = true,
            Profile = new ProfileExcerpt("Player",
                Guid.Empty),
            AccessToken = "access_token",
            OverrideId = "uuid",
            Xuid = string.Empty,
            UserType = "msa",
        });
    }
}