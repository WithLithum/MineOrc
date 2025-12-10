// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Runtime.Arguments;

namespace MineOrc.Tests;

public class ArgumentAssemblerTests
{
    [Fact]
    public void AssembleClasspath_Windows_AssembleCorrectly()
    {
        // Arrange
        IEnumerable<string> input =
        [
            "Path1",
            "Path2",
            "Path3",
        ];
        
        // Act
        var result = ArgumentAssembler.AssembleClasspath(input);
        
        // Assert
        var sp = Path.PathSeparator;
        Assert.Equal($"Path1{sp}Path2{sp}Path3",
            result);
    }
}