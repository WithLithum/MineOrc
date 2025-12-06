// Natverk - application server for Minecraft: Java Edition
// Copyright (C) 2025 WithLithum
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.CommandLine;
using JetBrains.Annotations;

namespace MineOrc.Commands;

public static class CommandHelper
{
    public static Option<bool> Switch([LocalizationRequired(false)] string shortName,
        [LocalizationRequired(false)] string longName,
        [LocalizationRequired] string description)
    {
        return new Option<bool>(shortName, longName)
        {
            Description = description
        };
    }
}