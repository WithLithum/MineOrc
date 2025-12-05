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
            { "foobar", false }
        };

        // Act
        var result = ArgumentConditions.MatchFeatures(ownFeatures, sourceFeatures);

        // Arrange
        Assert.True(result);
    }
}