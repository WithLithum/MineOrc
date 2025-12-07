// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Utilities;
using Spectre.Console;

namespace MineOrc.UI;

public class ProgressAction : IProgressEx<double>
{
    private readonly ProgressTask _progressTask;
    private readonly double _total;
    private readonly Lock _lock = new();

    public ProgressAction(ProgressTask progressTask, double total = 100)
    {
        _progressTask = progressTask;
        _total = total;
    }

    public void Report(double value)
    {
        lock (_lock)
        {
            var current = (value / _total) * 100;
            // Do not allow the current progress to go backwards
            if (current < _progressTask.Percentage)
            {
                return;
            }
            
            var difference = current - _progressTask.Percentage;

            _progressTask.Increment(difference);
        }
    }

    public void SetIntermediate(bool intermediate)
    {
        lock (_lock)
        {
            _progressTask.IsIndeterminate = intermediate;
        }
    }

    public void SetText(string text)
    {
        lock (_lock)
        {
            _progressTask.Description = text;
        }
    }

    public void Log(string text)
    {
        AnsiConsole.WriteLine(text);
    }
}