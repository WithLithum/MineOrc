// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using MineOrc.Foundation.Manifest.Options;
using MineOrc.Foundation.Runtime.Arguments;

namespace MineOrc.Tests;

public class ArgumentConditionTests
{
    [Fact]
    public void MatchFeatures_RequiredFeatureDoesNotExist_ReturnsFalse()
    {
        // Arrange
        string[] ownFeatures = [];
        var sourceFeatures = new Dictionary<string, bool>
        {
            { "required_feature", true }
        };

        // Act
        var result = ArgumentConditions.MatchFeatures(ownFeatures, sourceFeatures);

        // Arrange
        Assert.False(result);
    }

    [Fact]
    public void MatchFeatures_ProhibitedFeatureExists_ReturnsFalse()
    {
        // Arrange
        string[] ownFeatures = ["banned_feature"];
        var sourceFeatures = new Dictionary<string, bool>
        {
            { "banned_feature", false }
        };

        // Act
        var result = ArgumentConditions.MatchFeatures(ownFeatures, sourceFeatures);

        // Arrange
        Assert.False(result);
    }
    
    [Fact]
    public void MatchFeatures_RequiredFeatureExists_ReturnsTrue()
    {
        // Arrange
        string[] ownFeatures = ["required_feature"];
        var sourceFeatures = new Dictionary<string, bool>
        {
            { "required_feature", true }
        };

        // Act
        var result = ArgumentConditions.MatchFeatures(ownFeatures, sourceFeatures);

        // Arrange
        Assert.True(result);
    }
    
    [Fact]
    public void MatchFeatures_ProhibitedFeatureDoesNotExist_ReturnsTrue()
    {
        // Arrange
        string[] ownFeatures = [];
        var sourceFeatures = new Dictionary<string, bool>
        {
            { "banned_feature", false }
        };

        // Act
        var result = ArgumentConditions.MatchFeatures(ownFeatures, sourceFeatures);

        // Arrange
        Assert.True(result);
    }
    
    [Fact]
    public void MatchFeatures_ComplexMatchOkay_ReturnsTrue()
    {
        // Arrange
        // We have three rules playing below
        // 'foo': required feature, exists
        // 'bar': ignored feature
        // 'foobar': prohibited feature, does not exist
        string[] ownFeatures = ["foo", "bar"];
        var sourceFeatures = new Dictionary<string, bool>
        {
            { "foo", true },
            { "foobar", false },
        };

        // Act
        var result = ArgumentConditions.MatchFeatures(ownFeatures, sourceFeatures);

        // Arrange
        Assert.True(result);
    }
    
    [Theory]
    [InlineData(null, null, "x64", "windows")]
    [InlineData("x64", null, "x64", "windows")]
    [InlineData(null, "windows", "x64", "windows")]
    [InlineData("x64", "windows", "x64", "windows")]
    public void MatchPlatform_PassingConditions_Pass(string? checkArch,
        string? checkSystem,
        string ownArch,
        string ownSystem)
    {
        // Arrange
        var condition = new RuntimePlatformRule
        {
            Arch = checkArch,
            Name = checkSystem,
        };
        
        // Act
        var result = ArgumentConditions.MatchPlatform(condition,
            systemArch: ownArch,
            systemName: ownSystem);
        
        // Assert
        Assert.True(result);
    }
    
    [Theory]
    [InlineData("x64", null, "arm64", "linux")]
    [InlineData(null, "windows", "arm64", "linux")]
    [InlineData("x64", "windows", "arm64", "linux")]
    public void MatchPlatform_FailingConditions_Fail(string? checkArch,
        string? checkSystem,
        string ownArch,
        string ownSystem)
    {
        // Arrange
        var condition = new RuntimePlatformRule
        {
            Arch = checkArch,
            Name = checkSystem,
        };
        
        // Act
        var result = ArgumentConditions.MatchPlatform(condition,
            systemArch: ownArch,
            systemName: ownSystem);
        
        // Assert
        Assert.False(result);
    }
}