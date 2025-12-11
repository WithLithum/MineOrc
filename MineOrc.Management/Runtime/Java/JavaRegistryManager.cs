// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using MineOrc.Foundation.Json;
using MineOrc.Foundation.Runtime.Java;

namespace MineOrc.Management.Runtime.Java;

public sealed class JavaRegistryManager
{
    private readonly string _filePath;
    private Dictionary<string, JavaInfo> _infos = [];
    
    public JavaRegistryManager(string filePath)
    {
        _filePath = filePath;
    }
    
    public string? DefaultKey { get; set; }
    
    public IReadOnlyDictionary<string, JavaInfo> Items => _infos;

    public string? GetValidDefault()
    {
        if (string.IsNullOrWhiteSpace(DefaultKey)
            || !Items.ContainsKey(DefaultKey))
        {
            return null;
        }

        return DefaultKey;
    }
    
    public async Task LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }
        
        var stream = File.OpenRead(_filePath);
        
        JavaData? data;
        await using (stream.ConfigureAwait(false))
        {
            data = await JsonSerializer.DeserializeAsync(stream,
                    MineOrcFilesJsonContext.Default.JavaData)
                .ConfigureAwait(false);
        }
        
        if (data == null)
        {
            return;
        }

        _infos = new Dictionary<string, JavaInfo>(data.Runtimes);
    }

    public async Task SaveAsync()
    {
        var javaData = new JavaData(DefaultKey, _infos);
        
        var stream = File.Create(_filePath);
        await using (stream.ConfigureAwait(false))
        {
            await JsonSerializer.SerializeAsync(stream, javaData,
                MineOrcFilesJsonContext.Default.JavaData).ConfigureAwait(false);
        }
    }

    public void Add(string key, JavaInfo info)
    {
        _infos.Add(key, info);
    }

    public bool TryAdd(string key, JavaInfo info)
    {
        return _infos.TryAdd(key, info);
    }
}