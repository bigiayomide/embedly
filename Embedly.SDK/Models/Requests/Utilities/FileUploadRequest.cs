using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Embedly.SDK.Models.Requests.Utilities;

/// <summary>
///     Request model for file upload operations.
/// </summary>
public sealed class FileUploadRequest
{
    /// <summary>
    ///     Gets or sets the file content as a stream.
    /// </summary>
    [Required(ErrorMessage = "File content is required")]
    public Stream FileContent { get; set; } = Stream.Null;

    /// <summary>
    ///     Gets or sets the filename.
    /// </summary>
    [Required(ErrorMessage = "Filename is required")]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the content type (MIME type).
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    ///     Gets or sets the file description.
    /// </summary>
    public string? Description { get; set; }
}