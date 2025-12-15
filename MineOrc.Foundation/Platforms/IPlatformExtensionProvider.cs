// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Instancing;

namespace MineOrc.Foundation.Platforms;

public interface IPlatformExtensionProvider
{
    /// <summary>
    /// Queries available versions of the extension that supports the specified game version.
    /// </summary>
    /// <param name="gameVersion">The game version to query.</param>
    /// <param name="httpClient">The HTTP client to use.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An enumerable containing the query results.</returns>
    Task<IEnumerable<string>> GetVersionsAsync(string gameVersion,
        HttpClient httpClient,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Returns a new instance of <see cref="ProfileExtension"/> containing the libraries,
    /// arguments and main class information for the specified game and extension version.
    /// </summary>
    /// <param name="version">The extension version.</param>
    /// <param name="gameVersion">The game version.</param>
    /// <param name="httpClient">The HTTP client to use in network operations.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created profile extension.</returns>
    /// <exception cref="HttpRequestException">HTTP request failed for online operations.</exception>
    /// <exception cref="IOException">IO error occurred in online operations.</exception>
    /// <exception cref="OperationCanceledException">The operation was cancelled.</exception>
    Task<ProfileExtension> CreateExtensionAsync(string version,
        string gameVersion,
        HttpClient httpClient,
        CancellationToken cancellationToken = default);
}