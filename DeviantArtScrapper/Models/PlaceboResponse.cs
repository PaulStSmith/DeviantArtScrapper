using Newtonsoft.Json;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents the response from DeviantArt placebo endpoint used for testing API connectivity.
/// </summary>
public class PlaceboResponse
{
    /// <summary>
    /// Gets or sets the status message from the placebo endpoint.
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}
