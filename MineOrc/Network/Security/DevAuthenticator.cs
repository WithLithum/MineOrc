// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later
#if DEBUG

using MineOrc.Foundation.Network.Security;

namespace MineOrc.Network.Security;

internal static class DevAuthenticator
{
    // Means MineOrc:DevUser
    private static readonly Guid DevGuid = new("00070e19-1c23-3a31-b83f-464d545b6269");
    
    private sealed class DevProfile : ISessionProfile
    {
        public Guid Id => DevGuid;
        public string Name => "MineOrc_Dev";
    }
    
    internal static PlayAuthSession CreateSession()
    {
        return new PlayAuthSession(new DevProfile(), new OfflineAuthSession(),
            "MINEORC_DEV");
    }
}

#endif