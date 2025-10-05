using Newtonsoft.Json;
using DeviantArtScrapper.Converters;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents a DeviantArt deviation (artwork submission).
/// </summary>
public class Deviation
{
    /// <summary>
    /// Gets or sets the unique identifier for this deviation.
    /// </summary>
    [JsonProperty("deviationid")]
    public string DeviationId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the print ID if this deviation is available as a print.
    /// </summary>
    [JsonProperty("printid")]
    public string? PrintId { get; set; }
    
    /// <summary>
    /// Gets or sets the URL to view this deviation on DeviantArt.
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the title of the deviation.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether the current user has favorited this deviation.
    /// </summary>
    [JsonProperty("is_favourited")]
    public bool IsFavourited { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this deviation has been deleted.
    /// </summary>
    [JsonProperty("is_deleted")]
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this deviation is published.
    /// </summary>
    [JsonProperty("is_published")]
    public bool IsPublished { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this deviation is blocked.
    /// </summary>
    [JsonProperty("is_blocked")]
    public bool IsBlocked { get; set; }
    
    /// <summary>
    /// Gets or sets the author information for this deviation.
    /// </summary>
    [JsonProperty("author")]
    public Author Author { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the statistics (favorites, comments) for this deviation.
    /// </summary>
    [JsonProperty("stats")]
    public Stats Stats { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the date and time when this deviation was published.
    /// </summary>
    /// <remarks>
    /// Automatically converted from Unix timestamp to DateTime.
    /// </remarks>
    [JsonProperty("published_time")]
    [JsonConverter(typeof(UnixTimestampConverter))]
    public DateTime? PublishedTime { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether comments are allowed on this deviation.
    /// </summary>
    [JsonProperty("allows_comments")]
    public bool AllowsComments { get; set; }
    
    /// <summary>
    /// Gets or sets the preview image information.
    /// </summary>
    [JsonProperty("preview")]
    public ImageInfo? Preview { get; set; }
    
    /// <summary>
    /// Gets or sets the full content image information.
    /// </summary>
    [JsonProperty("content")]
    public ImageInfo? Content { get; set; }
    
    /// <summary>
    /// Gets or sets the list of thumbnail images in various sizes.
    /// </summary>
    [JsonProperty("thumbs")]
    public List<Thumbnail> Thumbs { get; set; } = [];
    
    /// <summary>
    /// Gets or sets a value indicating whether this deviation contains mature content.
    /// </summary>
    [JsonProperty("is_mature")]
    public bool IsMature { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether this deviation is available for download.
    /// </summary>
    [JsonProperty("is_downloadable")]
    public bool IsDownloadable { get; set; }
}
