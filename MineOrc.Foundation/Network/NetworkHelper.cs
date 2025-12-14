// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;

namespace MineOrc.Foundation.Network;

public static class NetworkHelper
{
    public static async Task<JsonDocument> AsJsonDocumentAsync(this HttpContent content,
        CancellationToken cancellationToken)
    {
        JsonDocument document;
        var stream = await content.ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);
        await using (stream.ConfigureAwait(false))
        {
            document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        return document;
    }
}