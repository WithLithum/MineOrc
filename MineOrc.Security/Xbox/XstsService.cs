// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using MineOrc.Foundation.Network;

namespace MineOrc.Security.Xbox;

/// <summary>
/// Provides helper methods for dealing with the Xbox Secure Token Service.
/// </summary>
public static class XstsService
{
    public static async Task<XboxLiveAuthResult> Authenticate(string xboxToken,
        HttpClient client,
        CancellationToken cancellationToken)
    {
        var requestPayload = new JsonObject
        {
            {
                "Properties", new JsonObject
                {
                    { "SandboxId", "RETAIL" },
                    { "UserTokens", new JsonArray { xboxToken } },
                }
            },
            { "RelyingParty", "rp://api.minecraftservices.com/" },
            { "TokenType", "JWT" },
        };

        var message = XboxLiveService.CreateMessage(
            "https://xsts.auth.xboxlive.com/xsts/authorize",
            requestPayload);
        
        var response = await client.SendAsync(message,
            cancellationToken).ConfigureAwait(false);
        var document = await response.Content.AsJsonDocumentAsync(cancellationToken).ConfigureAwait(false);
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            ThrowXstsErrorResult(document);
        }

        response.EnsureSuccessStatusCode();
        

        return XboxLiveService.InterpretResultPayloadInternal(document);
    }

    [DoesNotReturn]
    private static void ThrowXstsErrorResult(JsonDocument document)
    {
        throw XstsException.CreateFromCode(document.RootElement.GetProperty("XErr").GetInt64());
    }
    
    private static async Task InterpretXstsLiveResultInternalAsync(
        HttpResponseMessage response, CancellationToken cancellationToken)
    {
        
    }
}