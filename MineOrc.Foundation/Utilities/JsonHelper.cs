// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace MineOrc.Foundation.Utilities;

public static class JsonHelper
{
    public static async Task<T?> DeserializeFileAsync<T>(string filePath,
        JsonTypeInfo<T> jsonTypeInfo,
        CancellationToken cancellationToken = default)
    {
        var stream = File.OpenRead(filePath);
        await using (stream.ConfigureAwait(false))
        {
            return await JsonSerializer.DeserializeAsync(stream, jsonTypeInfo,
                cancellationToken).ConfigureAwait(false);
        }
    }
}