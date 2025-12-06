// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using MineOrc.Foundation.Manifest;

namespace MineOrc.Tests;

public class DeserializeTests
{
    [Fact]
    public void GameArgumentEntry_StringInput_ConvertsSuccessfully()
    {
        // Arrange
        const string source = "--foo";
        const string sourceJson = $"\"{source}\"";
        
        // Act
        var result = JsonSerializer.Deserialize(sourceJson,
            VersionManifestJsonContext.Default.GameArgumentEntry);
        
        // Assert
        Assert.NotNull(result);
        Assert.Multiple(() => Assert.Null(result.Rules),
            () => Assert.Single(result.Value, source));
    }
    
    [Fact]
    public void GameArgumentEntry_ArrayInput_ConvertsSuccessfully()
    {
        // Arrange
        const string sourceJson = $"[\"--foo\", \"--bar\"]";
        
        // Act
        var result = JsonSerializer.Deserialize(sourceJson,
            VersionManifestJsonContext.Default.GameArgumentEntry);
        
        // Assert
        Assert.NotNull(result);
        Assert.Multiple(() => Assert.Null(result.Rules),
            () => Assert.Collection(result.Value,
                foo => Assert.Equal("--foo", foo),
                bar => Assert.Equal("--bar", bar)));
    }
    
    [Fact]
    public void JvmArgumentEntry_StringInput_ConvertsSuccessfully()
    {
        // Arrange
        const string source = "-dFoo";
        const string sourceJson = $"\"{source}\"";
        
        // Act
        var result = JsonSerializer.Deserialize(sourceJson,
            VersionManifestJsonContext.Default.GameArgumentEntry);
        
        // Assert
        Assert.NotNull(result);
        Assert.Multiple(() => Assert.Null(result.Rules),
            () => Assert.Single(result.Value, source));
    }
    
    [Fact]
    public void JvmArgumentEntry_ArrayInput_ConvertsSuccessfully()
    {
        // Arrange
        const string sourceJson = $"[\"-dFoo\", \"-dBar\"]";
        
        // Act
        var result = JsonSerializer.Deserialize(sourceJson,
            VersionManifestJsonContext.Default.JvmArgumentEntry);
        
        // Assert
        Assert.NotNull(result);
        Assert.Multiple(() => Assert.Null(result.Rules),
            () => Assert.Collection(result.Value,
                foo => Assert.Equal("-dFoo", foo),
                bar => Assert.Equal("-dBar", bar)));
    }
}