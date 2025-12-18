// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.RegularExpressions;
using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Manifest.Resources;
using MineOrc.Foundation.Utilities;
using MineOrc.Management.Resources;
using Owasp.Untrust.BoxedPaths;
using Owasp.Untrust.BoxedPaths.IO;

namespace MineOrc.Management.Runtime.Assets;

public partial class AssetManager
{
    private readonly PathSandbox _indexesSandbox;
    private readonly PathSandbox _objectsSandbox;

    private static readonly Regex ObjectNameRegex = CreateObjectNameRegex();

    public AssetManager(string root)
    {
        RootDirectory = root;

        _indexesSandbox = PathSandbox.BoxRoot(Path.Combine(root, "indexes"));
        _objectsSandbox = PathSandbox.BoxRoot(Path.Combine(root, "objects"));
    }

    public string RootDirectory { get; }

    [GeneratedRegex("^[A-Fa-f0-9]{40}$")]
    private static partial Regex CreateObjectNameRegex();

    public static bool IsObjectNameValid(string id)
    {
        return ObjectNameRegex.IsMatch(id);
    }

    public bool HasAssetIndex(string index)
    {
        return BoxedFile.Exists(GetAssetIndexFile(index));
    }

    public bool HasAssetObject(AssetInfo asset)
    {
        return HasAssetObject(asset.Hash);
    }

    private bool HasAssetObject(string id)
    {
        return IsObjectNameValid(id) && BoxedFile.Exists(GetAssetObjectFile(id));
    }

    public void CreatePrefix(AssetInfo asset)
    {
        if (!IsObjectNameValid(asset.Hash))
        {
            throw new ArgumentException(ExceptionMessages.FormatAssetObjectIdInvalid(asset.Hash,
                    ObjectNameRegex),
                nameof(asset));
        }

        var parent = BoxedPath.Of(_objectsSandbox, asset.Hash[..2]);
        BoxedDirectory.CreateDirectory(parent);
    }

    public BoxedPath GetAssetObjectFile(AssetInfo asset)
    {
        return GetAssetObjectFile(asset.Hash);
    }

    private BoxedPath GetAssetObjectFile(string id)
    {
        if (!IsObjectNameValid(id))
        {
            throw new ArgumentException(ExceptionMessages.FormatAssetObjectIdInvalid(id,
                    ObjectNameRegex),
                nameof(id));
        }

        var prefix = id[..2];
        
        var fullPath = BoxedPath.Of(_objectsSandbox,
            $"{prefix}{Path.DirectorySeparatorChar}{id}");

        return fullPath;
    }

    public BoxedPath GetAssetIndexFile(string indexName)
    {
        return BoxedPath.Of(_indexesSandbox, $"{indexName}.json");
    }

    public async Task<AssetManifest?> GetAssetIndexAsync(string assetId,
        CancellationToken cancellationToken = default)
    {
        if (!HasAssetIndex(assetId))
        {
            return null;
        }

        var indexFile = GetAssetIndexFile(assetId);
        return await JsonHelper.DeserializeFileAsync(indexFile,
            VersionManifestJsonContext.Default.AssetManifest,
            cancellationToken).ConfigureAwait(false);
    }
}