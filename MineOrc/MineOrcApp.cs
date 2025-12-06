// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net.Http.Headers;
using System.Reflection;
using MineOrc.Foundation.Network;

namespace MineOrc;

public static class MineOrcApp
{
    public static readonly string BaseName = Path.GetFileNameWithoutExtension(Environment.ProcessPath)
        ?? Assembly.GetEntryAssembly()?.GetName().Name
        ?? "mineorc";
    
    public static readonly string Version = typeof(MineOrcApp).Assembly.GetName().Version?.ToString(3)
        ?? "0.0.0-unknown";
    
    public static readonly HttpClient HttpClient = new()
    {
        DefaultRequestHeaders =
        {
            UserAgent =
            {
                new ProductInfoHeaderValue(BaseName, Version)
            }
        }
    };

    public static readonly PistonMeta PistonMetaClient = new(HttpClient);
}