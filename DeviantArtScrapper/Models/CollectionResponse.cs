using Newtonsoft.Json;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents a paginated response from the DeviantArt collections (favorites) endpoint.
/// </summary>
public class CollectionResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether more results are available.
    /// </summary>
    [JsonProperty("has_more")]
    public bool HasMore { get; set; }
    
    /// <summary>
    /// Gets or sets the offset to use for retrieving the next page of results.
    /// </summary>
    [JsonProperty("next_offset")]
    public int? NextOffset { get; set; }
    
    /// <summary>
    /// Gets or sets the list of favorited deviation items in this page.
    /// </summary>
    [JsonProperty("results")]
    public List<Deviation> Results { get; set; } = [];
}
