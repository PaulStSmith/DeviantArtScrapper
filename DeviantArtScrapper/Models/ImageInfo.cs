using Newtonsoft.Json;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents information about an image (content or preview).
/// </summary>
public class ImageInfo
{
    /// <summary>
    /// Gets or sets the source URL of the image.
    /// </summary>
    [JsonProperty("src")]
    public string Src { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the height of the image in pixels.
    /// </summary>
    [JsonProperty("height")]
    public int Height { get; set; }
    
    /// <summary>
    /// Gets or sets the width of the image in pixels.
    /// </summary>
    [JsonProperty("width")]
    public int Width { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the image has transparency (alpha channel).
    /// </summary>
    [JsonProperty("transparency")]
    public bool Transparency { get; set; }
    
    /// <summary>
    /// Gets or sets the file size in bytes.
    /// </summary>
    [JsonProperty("filesize")]
    public int? FileSize { get; set; }
}
