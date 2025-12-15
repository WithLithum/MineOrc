// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Collections.Frozen;
using MineOrc.Foundation.Instancing;
using MineOrc.Tests.Utils;

namespace MineOrc.Tests;

public class ProfileOrchestratorTests
{
    [Fact]
    public void GetMainClass_NoExtensions_ReturnsManifestMainClass()
    {
        // Arrange
        var profile = new ProfileInfo
        {
            ClientVersion = "MockVersion",
            Created = DateTime.MinValue,
            Extensions = null,
        };

        // Act
        var result = ProfileOrchestrator.GetMainClass(FakeData.ExampleVersion,
            profile);

        // Assert
        Assert.Equal(FakeData.ExampleMainClass, result);
    }

    [Fact]
    public void GetMainClass_SingleExtension_ReturnsExtensionMainClass()
    {
        // Arrange
        var profile = new ProfileInfo
        {
            ClientVersion = "MockVersion",
            Created = DateTime.MinValue,
            Extensions = new Dictionary<string, ProfileExtension>
            {
                {
                    "example", new ProfileExtension
                    {
                        MainClass = "org.example.extension.ProfileClient",
                    }
                },
            },
        };

        // Act
        var result = ProfileOrchestrator.GetMainClass(FakeData.ExampleVersion,
            profile);

        // Assert
        Assert.Equal("org.example.extension.ProfileClient", result);
    }

    [Fact]
    public void GetMainClass_MultipleExtensions_ReturnsLastExtensionMainClass()
    {
        // Arrange
        var profile = new ProfileInfo
        {
            ClientVersion = "MockVersion",
            Created = DateTime.MinValue,
            Extensions = new Dictionary<string, ProfileExtension>
            {
                {
                    "example", new ProfileExtension
                    {
                        MainClass = "org.example.extension.ProfileClient",
                    }
                },
                {
                    "example2", new ProfileExtension
                    {
                        MainClass = "org.example.extension.SomeOtherExtension",
                    }
                },
                {
                    "example3", new ProfileExtension
                    {
                        MainClass = "com.example.extension.YetAnotherExtensionClient",
                    }
                },
            },
        };

        // Act
        var result = ProfileOrchestrator.GetMainClass(FakeData.ExampleVersion,
            profile);

        // Assert
        Assert.Equal("com.example.extension.YetAnotherExtensionClient", result);
    }
}