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