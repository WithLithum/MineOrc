// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Utilities.Maven;
using MineOrc.Tests.Utils;

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

    [Fact]
    public void ToUri_ExampleCoordinate_ParseCorrectly()
    {
        // Arrange
        var coordinate = new MavenCoordinate("org.example", "minecraft", "1.0.0");
        
        // Act
        var result = coordinate.ToUri(new Uri("https://repo.maven.apache.org/maven2/"), "jar");
        
        // Assert
        Assert.Equal("https://repo.maven.apache.org/maven2/org/example/minecraft/1.0.0/minecraft-1.0.0.jar",
            result.ToString());
    }

    [WindowsFact]
    public void ToPathWindows_ExampleCoordinate_ParseCorrectly()
    {
        // Arrange
        const string root = @"C:\Users\Example\.minecraft\libraries";
        var coordinate = new MavenCoordinate("org.example", "minecraft", "1.0.0");
        
        // Act
        var result = coordinate.ToPath(root, "jar");
        
        // Assert
        Assert.Equal($@"{root}\org\example\minecraft\1.0.0\minecraft-1.0.0.jar",
            result);
    }
    
    [UnixFact]
    public void ToPathUnix_ExampleCoordinate_ParseCorrectly()
    { 
        // Arrange
        const string root = @"/home/example/.minecraft/libraries";
        var coordinate = new MavenCoordinate("org.example", "minecraft", "1.0.0");
        
        // Act
        var result = coordinate.ToPath(root, "jar");
        
        // Assert
        Assert.Equal($@"{root}/org/example/minecraft/1.0.0/minecraft-1.0.0.jar",
            result);
    }
}