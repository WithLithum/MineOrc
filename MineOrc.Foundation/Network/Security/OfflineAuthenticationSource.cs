// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Security;

public sealed class OfflineAuthenticationSource : IAuthenticationSource
{
    private readonly string _userName;

    public OfflineAuthenticationSource(string userName)
    {
        _userName = userName;
    }

    public Task<AuthenticationResult> TryLoginSilentlyAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(new AuthenticationResult
        {
            AccessToken = "access_token",
            UserType = "msa",
            OverrideId = "uuid",
            Succeeded = true,
            Profile = new ProfileExcerpt(_userName, Guid.Empty),
            Xuid = "xuid",
        });
    }
}