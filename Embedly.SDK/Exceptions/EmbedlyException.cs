using System;
using System.Collections.Generic;
using Embedly.SDK.Helpers;

namespace Embedly.SDK.Exceptions;

/// <summary>
///     Base exception for all Embedly SDK exceptions.
/// </summary>
[Serializable]
public class EmbedlyException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EmbedlyException" /> class.
    /// </summary>
    public EmbedlyException() : base("An error occurred while processing the Embedly API request.")
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EmbedlyException" /> class with a specific message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public EmbedlyException(string message) : base(message)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EmbedlyException" /> class with a message and error code.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">The error code.</param>
    public EmbedlyException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EmbedlyException" /> class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public EmbedlyException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="EmbedlyException" /> class with full error details.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errorContext">Additional error context.</param>
    /// <param name="innerException">The inner exception.</param>
    public EmbedlyException(
        string message,
        string? errorCode = null,
        Dictionary<string, object?>? errorContext = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        ErrorContext = errorContext.AsReadOnly();
    }

    /// <summary>
    ///     Gets the unique error code associated with this exception.
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    ///     Gets additional context data about the error.
    /// </summary>
    public IReadOnlyDictionary<string, object?>? ErrorContext { get; }
}