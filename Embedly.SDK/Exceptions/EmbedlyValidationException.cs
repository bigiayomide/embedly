using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Embedly.SDK.Exceptions;

/// <summary>
/// Exception thrown when request validation fails.
/// </summary>
[Serializable]
public class EmbedlyValidationException : EmbedlyException
{
    /// <summary>
    /// Gets the validation errors.
    /// </summary>
    public IReadOnlyList<ValidationError> ValidationErrors { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EmbedlyValidationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    public EmbedlyValidationException(string message) 
        : base(message, "VALIDATION_ERROR")
    {
        ValidationErrors = [];
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EmbedlyValidationException"/> class with validation errors.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The validation errors.</param>
    public EmbedlyValidationException(string message, IEnumerable<ValidationError> errors) 
        : base(message, "VALIDATION_ERROR", CreateErrorContext(errors))
    {
        ValidationErrors = errors?.ToList() ?? new List<ValidationError>();
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EmbedlyValidationException"/> class from FluentValidation results.
    /// </summary>
    /// <param name="validationResult">The validation result.</param>
    public EmbedlyValidationException(ValidationResult validationResult)
        : this(
            CreateMessage(validationResult.Errors),
            validationResult.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage, e.AttemptedValue)))
    {
    }
    
    private static string CreateMessage(IList<ValidationFailure> failures)
    {
        if (failures == null || failures.Count == 0)
        {
            return "Validation failed.";
        }
        
        if (failures.Count == 1)
        {
            return $"Validation failed: {failures[0].ErrorMessage}";
        }
        
        var errors = string.Join("; ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
        return $"Validation failed with {failures.Count} errors: {errors}";
    }
    
    private static Dictionary<string, object?> CreateErrorContext(IEnumerable<ValidationError> errors)
    {
        var errorList = errors?.ToList() ?? new List<ValidationError>();
        return new Dictionary<string, object?>
        {
            ["ErrorCount"] = errorList.Count,
            ["Errors"] = errorList.Select(e => new
            {
                e.PropertyName,
                e.ErrorMessage,
                e.AttemptedValue
            }).ToList()
        };
    }
}

/// <summary>
/// Represents a validation error.
/// </summary>
public sealed record ValidationError(
    string PropertyName,
    string ErrorMessage,
    object? AttemptedValue = null);