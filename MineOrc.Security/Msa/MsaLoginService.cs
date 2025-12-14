// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using MineOrc.Foundation.Utilities;

namespace MineOrc.Security.Msa;

/// <summary>
/// Acts as a central piece of management in MSA premium authentication.
/// </summary>
public class MsaLoginService
{
    private static readonly string[] MsaScopes =
    [
        "XboxLive.signin",
        "XboxLive.offline_access",
    ];

    private readonly IPublicClientApplication _client;
    private readonly StorageCreationProperties _storageProperties;
    private MsalCacheHelper? _cacheHelper;

    public MsaLoginService(string clientId,
        ApplicationMeta meta)
    {
        _client = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority("https://login.microsoftonline.com/consumers/")
            .WithClientName(meta.Name)
            .WithClientVersion(meta.Version)
            .Build();

        _storageProperties = new StorageCreationPropertiesBuilder(meta.Name, $"{meta.Name}_Xbox")
            .WithLinuxKeyring(meta.Package,
                "MSAL",
                "Credentials for premium authentication",
                new KeyValuePair<string, string>("app_name", meta.Name),
                new KeyValuePair<string, string>("data_version", "v1"))
            .WithMacKeyChain(meta.Package,
                "Xbox_MSA")
            .Build();
    }

    public async Task InitializeAsync()
    {
        _cacheHelper = await MsalCacheHelper.CreateAsync(_storageProperties).ConfigureAwait(false);
        _cacheHelper.RegisterCache(_client.UserTokenCache);
    }

    private async Task<AuthenticationResult?> LoginSilentlyOrDefaultAsync(
        CancellationToken cancellationToken)
    {
        var accounts = await _client.GetAccountsAsync()
            .ConfigureAwait(false);

        try
        {
            return await _client.AcquireTokenSilent(MsaScopes, accounts.FirstOrDefault())
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        catch (MsalUiRequiredException)
        {
            return null;
        }
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
    public async Task<AuthenticationResult> SignInWithDeviceCodeAsync(
        Func<DeviceCodeResult, Task> showDeviceCode,
        CancellationToken cancellationToken = default)
    {
        return await LoginSilentlyOrDefaultAsync(cancellationToken).ConfigureAwait(false)
               ?? await _client.AcquireTokenWithDeviceCode(MsaScopes,
                       showDeviceCode)
                   .ExecuteAsync(cancellationToken).ConfigureAwait(false);
    }
}