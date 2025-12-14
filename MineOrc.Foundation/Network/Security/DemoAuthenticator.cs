// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Security;

public static class DemoAuthenticator
{
    public static PlayAuthSession CreateSession()
    {
        return new PlayAuthSession(new DemoProfile(),
            new OfflineAuthSession(),
            "DEMO_MODE_USER");
    }
    
    private sealed class DemoProfile : ISessionProfile
    {
        public Guid Id => Guid.Empty;
        public string Name => "Player";
    }
}