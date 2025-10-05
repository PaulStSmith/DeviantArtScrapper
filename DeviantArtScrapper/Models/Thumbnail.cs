using Newtonsoft.Json;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents a thumbnail image with dimensions.
/// </summary>
public class Thumbnail
{
    /// <summary>
    /// Gets or sets the source URL of the thumbnail.
    /// </summary>
    [JsonProperty("src")]
    public string Src { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the height of the thumbnail in pixels.
    /// </summary>
    [JsonProperty("height")]
    public int Height { get; set; }
    
    /// <summary>
    /// Gets or sets the width of the thumbnail in pixels.
    /// </summary>
    [JsonProperty("width")]
    public int Width { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the thumbnail has transparency (alpha channel).
    /// </summary>
    [JsonProperty("transparency")]
    public bool Transparency { get; set; }
}