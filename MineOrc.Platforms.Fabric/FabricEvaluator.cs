// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Instancing;
using MineOrc.Foundation.Manifest.Libraries;
using MineOrc.Foundation.Runtime;
using MineOrc.Platforms.Fabric.Meta;

namespace MineOrc.Platforms.Fabric;

public static class FabricEvaluator
{
    private static readonly Uri FabricMaven = new("https://maven.fabricmc.net/");

    public static async Task<ProfileExtension> CreateExtensionOnlineAsync(FabricLoaderMeta meta,
        HttpClient client,
        CancellationToken cancellationToken = default)
    {
        var launcher = meta.LauncherMeta;
        var commonLibs = launcher.Libraries.Common.Select(ConvertLibraryInternal)
            .Append(await ConvertReferenceOnlineInternalAsync(meta.Loader,
                    client,
                    cancellationToken)
                .ConfigureAwait(false))
            .Append(await ConvertReferenceOnlineInternalAsync(meta.Intermediary,
                    client,
                    cancellationToken)
                .ConfigureAwait(false));
        var clientLibs = launcher.Libraries.Client.Select(ConvertLibraryInternal);
        var mainClass = launcher.MainClass;

        return new ProfileExtension
        {
            Libraries = commonLibs.Concat(clientLibs).ToArray(),
            MainClass = mainClass.Client,
            JvmArguments =
            [
                "-DFabricMcEmu= net.minecraft.client.main.Main ",
            ],
        };
    }

    private static async Task<LibraryInfo> ConvertReferenceOnlineInternalAsync(
        FabricVersionRef fabric,
        HttpClient client,
        CancellationToken cancellationToken = default)
    {
        const string shaExtension = "jar.sha1";
        const string jarExtension = "jar";

        var sha1Url = fabric.Maven.ToUri(FabricMaven, shaExtension);
        var jarUrl = fabric.Maven.ToUri(FabricMaven, jarExtension);

        var sha1 = await GetSha1InternalAsync(sha1Url, client, cancellationToken)
            .ConfigureAwait(false);
        var length = await GetContentLengthInternalAsync(jarUrl, client, cancellationToken)
            .ConfigureAwait(false);

        var artefact = new LibraryArtefactInfo
        {
            Path = fabric.Maven.ToArtefactPath(jarExtension),
            Url = fabric.Maven.ToUri(FabricMaven, jarExtension),
            Size = length,
            Sha1 = sha1,
        };

        return new LibraryInfo
        {
            Name = fabric.Maven,
            Downloads = new LibraryDownloadInfo
            {
                Artifact = artefact,
            },
        };
    }

    private static async Task<int?> GetContentLengthInternalAsync(Uri url,
        HttpClient client,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            return null;
        }

        if (response.IsSuccessStatusCode)
        {
            return null;
        }

        var length = response.Content.Headers.ContentLength;
        return length < int.MaxValue
            ? (int)length
            : null;
    }

    private static async Task<string?> GetSha1InternalAsync(Uri url,
        HttpClient client,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);
        }
        catch (HttpRequestException)
        {
            return null;
        }

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadAsStringAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static LibraryInfo ConvertLibraryInternal(FabricLibraryInfo fabric)
    {
        const string extension = "jar";
        var rootUrl = fabric.Url ?? LibraryEvaluator.MinecraftLibraries;
        var artefact = new LibraryArtefactInfo
        {
            Path = fabric.Name.ToArtefactPath(extension),
            Url = fabric.Name.ToUri(rootUrl, extension),
            Size = fabric.Size,
            Sha1 = fabric.Sha1,
        };

        return new LibraryInfo
        {
            Name = fabric.Name,
            Downloads = new LibraryDownloadInfo
            {
                Artifact = artefact,
            },
        };
    }
}