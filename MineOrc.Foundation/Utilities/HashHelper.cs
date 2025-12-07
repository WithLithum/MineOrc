// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Security.Cryptography;
using JetBrains.Annotations;

namespace MineOrc.Foundation.Utilities;

public static class HashHelper
{
    public static async Task<bool> VerifyStreamAsync([HandlesResourceDisposal] Stream stream,
        string sha1,
        CancellationToken cancellationToken = default)
    {
        string localHash;
        await using (stream.ConfigureAwait(false))
        {
            localHash = Convert.ToHexStringLower(await SHA1.HashDataAsync(stream,
                cancellationToken).ConfigureAwait(false));
        }

        return sha1.Equals(localHash, StringComparison.OrdinalIgnoreCase);
    }
    
    public static async ValueTask<bool> VerifyFileAsync(string path,
        string sha1,
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        var stream = File.OpenRead(path);
        await using (stream.ConfigureAwait(false))
        {
            return await VerifyStreamAsync(stream, sha1, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}