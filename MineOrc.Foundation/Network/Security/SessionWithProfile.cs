// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Security;

public sealed record SessionWithProfile(ISessionProfile Profile,
    IAuthSession Session,
    IAuthDetail? Detail = null);