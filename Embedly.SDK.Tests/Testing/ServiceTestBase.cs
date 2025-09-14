using System;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Responses.Common;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Testing;

/// <summary>
///     Base class for service unit tests with common API response testing patterns.
/// </summary>
public abstract class ServiceTestBase : TestBase
{
    /// <summary>
    ///     Verifies that a service method throws ArgumentNullException when passed null request.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="serviceMethod">The service method to test.</param>
    protected static void AssertThrowsArgumentNullExceptionForNullRequest<TRequest, TResponse>(
        Func<TRequest, CancellationToken, Task<ApiResponse<TResponse>>> serviceMethod)
        where TRequest : class
        where TResponse : class
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentNullException>(() => serviceMethod(null!, CancellationToken.None));

        exception.Should().NotBeNull();
        exception!.ParamName.Should().Be("request");
    }

    /// <summary>
    ///     Verifies that a service method throws ArgumentException when passed null or empty string parameter.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="serviceMethod">The service method to test.</param>
    /// <param name="expectedParamName">The expected parameter name in the exception.</param>
    protected static void AssertThrowsArgumentExceptionForNullOrEmptyString<TResponse>(
        Func<string, CancellationToken, Task<ApiResponse<TResponse>>> serviceMethod,
        string expectedParamName)
        where TResponse : class
    {
        // Test null string
        var nullException = Assert.ThrowsAsync<ArgumentException>(() => serviceMethod(null!, CancellationToken.None));
        nullException.Should().NotBeNull();
        nullException!.ParamName.Should().Be(expectedParamName);

        // Test empty string
        var emptyException =
            Assert.ThrowsAsync<ArgumentException>(() => serviceMethod(string.Empty, CancellationToken.None));
        emptyException.Should().NotBeNull();
        emptyException!.ParamName.Should().Be(expectedParamName);

        // Test whitespace string
        var whitespaceException =
            Assert.ThrowsAsync<ArgumentException>(() => serviceMethod("   ", CancellationToken.None));
        whitespaceException.Should().NotBeNull();
        whitespaceException!.ParamName.Should().Be(expectedParamName);
    }

    /// <summary>
    ///     Creates a successful API response for testing.
    /// </summary>
    /// <typeparam name="T">The response data type.</typeparam>
    /// <param name="data">The response data.</param>
    /// <param name="message">Optional success message.</param>
    /// <returns>A successful API response.</returns>
    protected static ApiResponse<T> CreateSuccessfulApiResponse<T>(T data, string? message = null)
        where T : class
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message ?? "Operation completed successfully",
            RequestId = "test-request-id",
            Timestamp = CreateTestTimestamp()
        };
    }

    /// <summary>
    ///     Creates a failed API response for testing.
    /// </summary>
    /// <typeparam name="T">The response data type.</typeparam>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">Optional error code.</param>
    /// <returns>A failed API response.</returns>
    protected static ApiResponse<T> CreateFailedApiResponse<T>(string message, string? errorCode = null)
        where T : class
    {
        return new ApiResponse<T>
        {
            Success = false,
            Data = null,
            Message = message,
            Error = errorCode != null ? new ErrorDetails { Code = errorCode, Message = message } : null,
            RequestId = "test-request-id",
            Timestamp = CreateTestTimestamp()
        };
    }

    /// <summary>
    ///     Verifies that HTTP client was called with the correct parameters.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="expectedUrlContains">String that should be contained in the URL.</param>
    /// <param name="request">The expected request object.</param>
    /// <param name="times">How many times the call should have been made.</param>
    protected void VerifyHttpClientPostCall<TRequest, TResponse>(
        string expectedUrlContains,
        TRequest request,
        Times? times = null)
        where TRequest : class
        where TResponse : class
    {
        MockHttpClient.Verify(
            x => x.PostAsync<TRequest, TResponse>(
                It.Is<string>(url => url.Contains(expectedUrlContains)),
                request,
                It.IsAny<CancellationToken>()),
            times ?? Times.Once());
    }

    /// <summary>
    ///     Verifies that HTTP client GET was called with the correct parameters.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="expectedUrlContains">String that should be contained in the URL.</param>
    /// <param name="times">How many times the call should have been made.</param>
    protected void VerifyHttpClientGetCall<TResponse>(
        string expectedUrlContains,
        Times? times = null)
        where TResponse : class
    {
        MockHttpClient.Verify(
            x => x.GetAsync<TResponse>(
                It.Is<string>(url => url.Contains(expectedUrlContains)),
                It.IsAny<CancellationToken>()),
            times ?? Times.Once());
    }

    /// <summary>
    ///     Verifies that HTTP client PATCH was called with the correct parameters.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="expectedUrlContains">String that should be contained in the URL.</param>
    /// <param name="request">The expected request object.</param>
    /// <param name="times">How many times the call should have been made.</param>
    protected void VerifyHttpClientPatchCall<TRequest, TResponse>(
        string expectedUrlContains,
        TRequest request,
        Times? times = null)
        where TRequest : class
        where TResponse : class
    {
        MockHttpClient.Verify(
            x => x.PatchAsync<TRequest, TResponse>(
                It.Is<string>(url => url.Contains(expectedUrlContains)),
                request,
                It.IsAny<CancellationToken>()),
            times ?? Times.Once());
    }
}