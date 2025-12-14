// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Utilities.Maven;

namespace MineOrc.Tests;

public class MavenCoordinateTests
{
    [Fact]
    public void Parse_CorrectFormat_Succeed()
    {
        // Arrange
        const string input = "org.example:test:1.0.0";
        
        // Act
        var result = MavenCoordinate.Parse(input, null);
        
        // Assert
        Assert.Multiple(() => Assert.Equal("org.example", result.Group),
            () => Assert.Equal("test", result.Artefact),
            () => Assert.Equal("1.0.0", result.Version));
    }
    
    [Fact]
    public void Parse_TooManySegments_Fail()
    {
        // Arrange
        const string input = "org.example:test:1.0.0:more";
        
        // Act
        var success = MavenCoordinate.TryParse(input, null, out _);
        
        // Assert
        Assert.False(success);
    }
    
    [Fact]
    public void Parse_TooFewSegments_Fail()
    {
        // Arrange
        const string input = "org.example:test";
        
        // Act
        var success = MavenCoordinate.TryParse(input, null, out _);
        
        // Assert
        Assert.False(success);
    }
}