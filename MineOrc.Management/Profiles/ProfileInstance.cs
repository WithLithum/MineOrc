// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using MineOrc.Foundation.Instancing;
using MineOrc.Foundation.Json;

namespace MineOrc.Management.Profiles;

public class ProfileInstance
{
    private readonly string _metadataPath;

    public ProfileInstance(string profileDir, string metadataPath, ProfileInfo metadata)
    {
        Root = profileDir;
        Metadata = metadata;
        _metadataPath = metadataPath;
    }

    public string Root { get; }
    
    public ProfileInfo Metadata { get; private set; }

    public async Task UpdateMetadataAsync(ProfileInfo profileInfo)
    {
        var stream = File.Create(_metadataPath);
        
        await using (stream.ConfigureAwait(false))
        {
            await JsonSerializer.SerializeAsync(stream,
                profileInfo,
                MineOrcFilesJsonContext.Default.ProfileInfo).ConfigureAwait(false);
        }
        
        Metadata = profileInfo;
    }
    
    public string GetFolderPath(ProfileFolder folder)
    {
        return folder switch
        {
            ProfileFolder.Root => Root,
            ProfileFolder.ModConfiguration => Path.Combine(Root, "config"),
            ProfileFolder.Debug => Path.Combine(Root, "debug"),
            ProfileFolder.Logs => Path.Combine(Root, "logs"),
            ProfileFolder.Mods => Path.Combine(Root, "mods"),
            ProfileFolder.ResourcePacks => Path.Combine(Root, "resourcepacks"),
            ProfileFolder.Saves => Path.Combine(Root, "saves"),
            ProfileFolder.Screenshots => Path.Combine(Root, "screenshots"),
            _ => throw new ArgumentOutOfRangeException(nameof(folder), folder, null),
        };
    }
}