// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Runtime;

public enum VerifyResult
{
    /// <summary>
    /// The local file is intact.
    /// </summary>
    Intact,
    /// <summary>
    /// The local file is damaged.
    /// </summary>
    Damaged,
    /// <summary>
    /// The artefact has no hash specified.
    /// </summary>
    NoHash,
}