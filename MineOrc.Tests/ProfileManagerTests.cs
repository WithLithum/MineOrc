// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Instancing;
using MineOrc.Management.Profiles;

namespace MineOrc.Tests;

public class ProfileManagerTests
{
    [Theory]
    [InlineData("profile_name")]
    [InlineData("Profile_Name")]
    [InlineData("PROFILE")]
    [InlineData("latest-version")]
    public void ProfileNameRegex_GivenValidName_Passes(string value)
    {
        // Arrange
        var regex = ProfileManager.ProfileNameRegex;
        
        // Act
        var result = regex.IsMatch(value);
        
        // Assert
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("profile name")]
    [InlineData("Profile Name")]
    [InlineData("/profile name/")]
    [InlineData("$ Profile")]
    public void ProfileNameRegex_GivenInvalidName_Fails(string value)
    {
        // Arrange
        var regex = ProfileManager.ProfileNameRegex;
        
        // Act
        var result = regex.IsMatch(value);
        
        // Assert
        Assert.False(result);
    }
}