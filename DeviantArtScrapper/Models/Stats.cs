using Newtonsoft.Json;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents statistics for a deviation (favorites, comments).
/// </summary>
public class Stats
{
    /// <summary>
    /// Gets or sets the number of comments on the deviation.
    /// </summary>
    [JsonProperty("comments")]
    public int Comments { get; set; }
    
    /// <summary>
    /// Gets or sets the number of favorites the deviation has received.
    /// </summary>
    [JsonProperty("favourites")]
    public int Favourites { get; set; }
}
