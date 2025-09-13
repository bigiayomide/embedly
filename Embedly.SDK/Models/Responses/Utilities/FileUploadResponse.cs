using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Utilities;

/// <summary>
/// Represents the response from a file upload operation.
/// </summary>
public sealed class FileUploadResponse
{
    /// <summary>
    /// Gets or sets the uploaded file URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the file ID.
    /// </summary>
    [JsonPropertyName("fileId")]
    public string FileId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the original filename.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string FileName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the file size in bytes.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public long FileSize { get; set; }
    
    /// <summary>
    /// Gets or sets the content type.
    /// </summary>
    [JsonPropertyName("contentType")]
    public string? ContentType { get; set; }
}