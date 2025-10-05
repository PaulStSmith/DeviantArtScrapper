using Newtonsoft.Json;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents an error response from the DeviantArt API.
/// </summary>
public class DeviantArtError
{
    /// <summary>
    /// Gets or sets the error code or type.
    /// </summary>
    [JsonProperty("error")]
    public string Error { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the human-readable error description.
    /// </summary>
    [JsonProperty("error_description")]
    public string ErrorDescription { get; set; } = string.Empty;
}
