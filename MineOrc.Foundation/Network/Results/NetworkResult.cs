// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

namespace MineOrc.Foundation.Network.Results;

public abstract record NetworkResult
{
    public static NetworkResult Ok => OkNetworkResult.Instance;

    /// <summary>
    /// Creates a new instance of <see cref="NetworkResult"/> from an instance of either
    /// <see cref="HttpIOException"/>, <see cref="HttpRequestException"/> or
    /// <see cref="IOException"/>.
    /// </summary>
    /// <param name="exception">
    /// The exception. Must be of <see cref="HttpIOException"/>, <see cref="HttpRequestException"/>
    /// or <see cref="IOException"/>.
    /// </param>
    /// <returns>
    /// An instance of <see cref="NetworkResult"/>, with its value corresponding to the exception
    /// type and state.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// An exception not of the allowed types is specified.
    /// </exception>
    public static NetworkResult FromException(Exception exception)
    {
        return exception switch
        {
            HttpIOException httpIo => FromExceptionInternal(httpIo),
            IOException io => FromExceptionInternal(io),
            HttpRequestException httpRequest => FromExceptionInternal(httpRequest),
            _ => throw new InvalidOperationException("Cannot handle the below exception as a result here", exception)
        };
    }
    
    private static IoErrorNetworkResult FromExceptionInternal(IOException exception)
    {
        return new IoErrorNetworkResult(exception.Message);
    }
    
    private static HttpErrorNetworkResult FromExceptionInternal(HttpIOException exception)
    {
        return new HttpErrorNetworkResult(exception.Message, exception.HttpRequestError);
    }

    private static NetworkResult FromExceptionInternal(HttpRequestException exception)
    {
        if (exception.StatusCode.HasValue)
        {
            return new HttpStatusNetworkResult(exception.StatusCode.Value);
        }
        
        return new HttpErrorNetworkResult(exception.Message, exception.HttpRequestError);
    }
}