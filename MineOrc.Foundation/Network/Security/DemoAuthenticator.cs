// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Security;

public static class DemoAuthenticator
{
    public static SessionWithProfile CreateSession()
    {
        return new SessionWithProfile(new DemoProfile(),
            new OfflineAuthSession());
    }
    
    private sealed class DemoProfile : ISessionProfile
    {
        public Guid Id => Guid.Empty;
        public string Name => "Player";
    }
}