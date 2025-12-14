// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest;
using MineOrc.Foundation.Manifest.Libraries;

namespace MineOrc.Foundation.Instancing;

public static class ProfileOrchestrator
{
    public static string GetMainClass(ClientManifest version,
        ProfileInfo profile)
    {
        if (profile.Extensions == null || profile.Extensions.Count == 0)
        {
            return version.MainClass;
        }

        var temp = profile.Extensions.LastOrDefault(x => x.MainClass != null);
        return temp?.MainClass ?? version.MainClass;
    }
    
    public static IEnumerable<LibraryInfo> GetLibraries(ClientManifest version,
        ProfileInfo profile)
    {
        IEnumerable<LibraryInfo> result = version.Libraries;
        
        if (profile.Extensions == null || profile.Extensions.Count == 0)
        {
            return result;
        }
        
        foreach (var extension in profile.Extensions)
        {
            if (extension.Libraries is null)
            {
                continue;
            }
            
            result = result.Concat(extension.Libraries);
        }
        
        return result;
    }
}