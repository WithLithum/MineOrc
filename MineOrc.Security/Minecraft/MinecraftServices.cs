// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using MineOrc.Foundation.Network.Security;
using MineOrc.Security.Minecraft.Profile;
using MineOrc.Security.Xbox;

namespace MineOrc.Security.Minecraft;

public static class MinecraftServices
{
    public static async Task<MinecraftServiceAuthResult> AuthenticateAsync(string xstsToken,
        string xboxHash,
        HttpClient client,
        CancellationToken cancellationToken)
    {
        var payload = new JsonObject
        {
            { "identityToken", $"XBL3.0 x={xboxHash};{xstsToken}" },
        };
        
        var message = XboxLiveService.CreateMessage("https://api.minecraftservices.com/authentication/login_with_xbox",
            payload);
        var response = await client.SendAsync(message, cancellationToken)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        
        return (await response.Content
            .ReadFromJsonAsync(MinecraftServicesJsonContext.Default.MinecraftServiceAuthResult,
                cancellationToken)
            .ConfigureAwait(false))!;
    }

    public static HttpRequestMessage CreateAuthenticatedGet(string url,
        IAuthSession session)
    {
        return new HttpRequestMessage(HttpMethod.Get, url)
        {
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Bearer",
                    session.AccessToken),
            },
        };
    }

    public static async Task<SecurityProfile> GetAuthenticatedProfile(IAuthSession session,
        HttpClient client)
    {
        const string url = "https://api.minecraftservices.com/minecraft/profile";
        var message = CreateAuthenticatedGet(url, session);
        
        var response = await client.SendAsync(message)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return (await response.Content
            .ReadFromJsonAsync(MinecraftServicesJsonContext.Default.SecurityProfile,
                cancellationToken: CancellationToken.None)
            .ConfigureAwait(false))!;
    }
}