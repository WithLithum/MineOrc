// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using JetBrains.Annotations;
using MineOrc.Foundation.Manifest.Resources;

namespace MineOrc.Foundation.Runtime.Resources;

public class AssetManager
{
    private readonly string _indexesDirectory;
    private readonly string _objectsDirectory;

    public AssetManager(string root)
    {
        RootDirectory = root;
        
        _indexesDirectory = Path.Combine(root, "indexes");
        _objectsDirectory = Path.Combine(root, "objects");
    }

    public string RootDirectory { get; }

    public bool HasAssetIndex(string index)
    {
        return File.Exists(GetAssetIndexFile(index));
    }

    public bool HasAssetObject(AssetInfo asset)
    {
        return HasAssetObject(asset.Hash);
    }
    
    private bool HasAssetObject(string id)
    {
        return File.Exists(GetAssetObjectFile(id));
    }

    public string GetAssetObjectFile(AssetInfo asset)
    {
        return GetAssetObjectFile(asset.Hash);
    }
    
    private string GetAssetObjectFile(string id)
    {
        var prefix = id[..2];
        var fullPath = Path.Combine(_objectsDirectory,
            prefix,
            id);

        return fullPath;
    }
    
    public string GetAssetIndexFile(string indexName)
    {
        return Path.GetFullPath($"{indexName}.json",
            _indexesDirectory);
    }
}