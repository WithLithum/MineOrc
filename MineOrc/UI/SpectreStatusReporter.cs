// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Utilities;
using Spectre.Console;

namespace MineOrc.UI;

public class SpectreStatusReporter : IStatusReporter
{
    private readonly StatusContext _context;
    private readonly Lock _lock = new();
    private readonly string? _prefix;

    public SpectreStatusReporter(StatusContext context, string? prefix = null)
    {
        _context = context;
        _prefix = prefix;
    }

    public void SetText(string text)
    {
        var endText = _prefix != null
            ? $"{_prefix}{text}"
            : text;
        
        lock (_lock)
        {
            _context.Status = endText;
        }
    }
}