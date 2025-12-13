// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Identity.Client;

namespace MineOrc.Security.Msa;

/// <summary>
/// Acts as a central piece of management in MSA premium authentication.
/// </summary>
public class SecureAuthority
{
    private readonly string _clientId;
    private readonly string _tenantId;
    private readonly IPublicClientApplication _client;

    public SecureAuthority(string clientId, string tenantId,
        string clientName,
        string clientVersion)
    {
        _clientId = clientId;
        _tenantId = tenantId;

        _client = PublicClientApplicationBuilder.Create(_clientId)
            .WithAuthority($"https://login.microsoftonline.com/consumers/")
            .WithClientName(clientName)
            .WithClientVersion(clientVersion)
            .Build();
    }

    /// <summary>
    /// Attempts to authenticate with Microsoft using the device code flow.
    /// </summary>
    /// <param name="showDeviceCode">The callback that is used to show the device code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The authentication result.</returns>
    /// <exception cref="MsalServiceException">There was a service error with the tenant.</exception>
    /// <exception cref="MsalClientException">The login operation has expired.</exception>
    /// <exception cref="OperationCanceledException">The operation was cancelled.</exception>
    public async Task<AuthenticationResult> SignInWithDeviceCodeAsync(Func<DeviceCodeResult,Task> showDeviceCode,
        CancellationToken cancellationToken = default)
    {
        return await _client.AcquireTokenWithDeviceCode(["XboxLive.signin"],
            showDeviceCode)
            .ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }
}