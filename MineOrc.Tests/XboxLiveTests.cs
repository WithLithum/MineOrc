// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using MineOrc.Security.Xbox;

namespace MineOrc.Tests;

public class XboxLiveTests
{
    [Fact]
    public void InterpretResultPayload_SampleData_ParsesCorrectly()
    {
        // Arrange
        const string sampleData = """
                                  {
                                      "IssueInstant": "2020-12-07T19:52:08.4463796Z",
                                      "NotAfter": "2020-12-21T19:52:08.4463796Z",
                                      "Token": "MY_TOKEN",
                                      "DisplayClaims": {
                                          "xui": [
                                              {
                                                  "uhs": "MY_HASH"
                                              }
                                          ]
                                      }
                                  }
                                  """;
        var document = JsonDocument.Parse(sampleData);

        // Act
        var result = XboxLiveService.InterpretResultPayloadInternal(document);

        // Assert
        Assert.Multiple(() => Assert.Equal("MY_TOKEN", result.Token),
            () => Assert.Equal("MY_HASH", result.UserHash));
    }
}