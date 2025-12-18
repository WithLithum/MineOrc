// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Management.Runtime.Assets;

namespace MineOrc.Tests;

public class AssetManagerTests
{
    [Fact]
    public void IsObjectNameValid_ValidObjectName_ReturnsTrue()
    {
        // Arrange
        const string value = "2e4ae90b012136efded076e6b85c125783cedcc2";
        
        // Act
        var result = AssetManager.IsObjectNameValid(value);
        
        // Assert
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("This is not hash")]
    [InlineData("../../../bin")]
    [InlineData(@"..\..\..\..\Windows")]
    public void IsObjectNameValid_Path_ReturnsFalse(string value)
    {
        // Act
        var result = AssetManager.IsObjectNameValid(value);
        
        // Assert
        Assert.False(result);
    }
}