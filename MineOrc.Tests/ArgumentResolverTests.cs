// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Frozen;
using MineOrc.Foundation.Runtime.Arguments;

namespace MineOrc.Tests;

public class ArgumentResolverTests
{
    [Fact]
    public void Resolver_NoVariable_ReturnAsIs()
    {
        // Arrange
        const string testValue = "No variable for you!";
        var resolver = new ArgumentValueResolver(FrozenDictionary<string, string>.Empty);
        
        // Act
        var resolveResult = resolver.Resolve(testValue);
        
        // Assert
        Assert.Equal(testValue, resolveResult);
    }
    
    [Fact]
    public void Resolver_HasDollarSignButActuallyNotVariable_ReturnAsIs()
    {
        // Arrange
        const string testValue = "This string contains $ symbol";
        var resolver = new ArgumentValueResolver(FrozenDictionary<string, string>.Empty);
        
        // Act
        var resolveResult = resolver.Resolve(testValue);
        
        // Assert
        Assert.Equal(testValue, resolveResult);
    }
    
    [Fact]
    public void Resolver_EntireVariableInsertion_ParseCorrectly()
    {
        // Arrange
        const string testValue = "${auth_access_token}";
        
        var dictionary = new Dictionary<string, string>
        {
            { "auth_access_token", "MY_ACCESS_TOKEN" }
        };
        var resolver = new ArgumentValueResolver(dictionary);
        
        // Act
        var resolveResult = resolver.Resolve(testValue);
        
        // Assert
        Assert.Equal("MY_ACCESS_TOKEN", resolveResult);
    }
    
    [Fact]
    public void Resolver_InsertionInMiddle_ParseCorrectly()
    {
        // Arrange
        const string testValue = "-Djava.library.path=${natives_directory}";
        
        var dictionary = new Dictionary<string, string>()
        {
            { "natives_directory", "MY_OWN_NATIVES" }
        };
        var resolver = new ArgumentValueResolver(dictionary);
        
        // Act
        var resolveResult = resolver.Resolve(testValue);
        
        // Assert
        Assert.Equal("-Djava.library.path=MY_OWN_NATIVES", resolveResult);
    }
}