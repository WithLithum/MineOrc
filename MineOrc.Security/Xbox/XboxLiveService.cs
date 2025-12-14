// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MineOrc.Security.Xbox;

/// <summary>
/// Provides utility methods for requesting authentication with Xbox Live services.
/// </summary>
public static class XboxLiveService
{
    public static async Task<XboxLiveAuthResult> Authenticate(string msaToken,
        HttpClient client,
        CancellationToken cancellationToken)
    {
        var requestPayload = new JsonObject
        {
            {
                "Properties", new JsonObject
                {
                    { "AuthMethod", "RPS" },
                    { "SiteName", "user.auth.xboxlive.com" },
                    { "RpsTicket", $"d={msaToken}" },
                }
            },
            { "RelyingParty", "http://auth.xboxlive.com" },
            { "TokenType", "JWT" },
        };

        var message = CreateMessage("https://user.auth.xboxlive.com/user/authenticate",
            requestPayload);

        var response = await client.SendAsync(message,
            cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        // Parse payload
        return await InterpretXboxLiveResultInternalAsync(response, cancellationToken)
            .ConfigureAwait(false);
    }

    internal static HttpRequestMessage CreateMessage(string uri, JsonObject requestPayload)
    {
        var message = new HttpRequestMessage(HttpMethod.Post,
            uri)
        {
            Content = new StringContent(requestPayload.ToJsonString(),
                Encoding.UTF8,
                MediaTypeNames.Application.Json),
            Headers =
            {
                Accept =
                {
                    new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)
                },
            },
        };
        return message;
    }

    private static async Task<XboxLiveAuthResult> InterpretXboxLiveResultInternalAsync(
        HttpResponseMessage response, CancellationToken cancellationToken)
    {
        JsonDocument document;
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);
        await using (stream.ConfigureAwait(false))
        {
            document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        return InterpretResultPayloadInternal(document);
    }

    internal static XboxLiveAuthResult InterpretResultPayloadInternal(JsonDocument document)
    {
        var json = document.RootElement;

        return new XboxLiveAuthResult
        {
            IssuedAt = json.GetProperty("IssueInstant").GetDateTime(),
            ExpiresAt = json.GetProperty("NotAfter").GetDateTime(),
            Token = json.GetProperty("Token").GetString()!,
            UserHash = json.GetProperty("DisplayClaims")
                .GetProperty("xui")[0].GetProperty("uhs").GetString()!,
        };
    }
}