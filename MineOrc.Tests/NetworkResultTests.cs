// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Net;
using MineOrc.Foundation.Network.Results;

namespace MineOrc.Tests;

public class NetworkResultTests
{
    [Fact]
    public void IsOk_Success_ReturnsTrue()
    {
        // Arrange
        var input = NetworkResult.Ok;
        
        // Act
        var result = input.IsOk;
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsOk_Error_ReturnsFalse()
    {
        // Arrange
        var input = NetworkResult.FromException(new IOException("Bruh"));
        
        // Act
        var result = input.IsOk;
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void FromException_HttpRequestError_ReturnsHttpError()
    {
        // Arrange
        var input = new HttpRequestException(HttpRequestError.InvalidResponse);
        
        // Act
        var result = NetworkResult.FromException(input);
        
        // Assert
        Assert.IsType<HttpErrorNetworkResult>(result);
    }
    
    [Fact]
    public void FromException_HttpStatusError_ReturnsHttpStatus()
    {
        // Arrange
        var input = new HttpRequestException(HttpRequestError.Unknown,
            statusCode: HttpStatusCode.NotFound);
        
        // Act
        var result = NetworkResult.FromException(input);
        
        // Assert
        Assert.IsType<HttpStatusNetworkResult>(result);
    }

    [Fact]
    public void FromException_HttpIoError_ReturnsHttpError()
    {
        // Arrange
        var input = new HttpIOException(HttpRequestError.ConnectionError);
        
        // Act
        var result = NetworkResult.FromException(input);
        
        // Assert
        Assert.IsType<HttpErrorNetworkResult>(result);
    }
    
    [Fact]
    public void FromException_IoError_ReturnsHttpError()
    {
        // Arrange
        var input = new IOException("Some kind of IO Exception here");
        
        // Act
        var result = NetworkResult.FromException(input);
        
        // Assert
        Assert.IsType<IoErrorNetworkResult>(result);
    }
    
    [Fact]
    public void FromException_OtherThanAllowedExceptionType_Throws()
    {
        // Arrange
        var input = new Exception("BRUH");
        
        // Act
        var exception = Record.Exception(() => NetworkResult.FromException(input));
        
        // Assert
        Assert.IsType<InvalidOperationException>(exception);
    }
}