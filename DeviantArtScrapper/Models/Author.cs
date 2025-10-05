using Newtonsoft.Json;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents a DeviantArt user who authored a deviation.
/// </summary>
public class Author
{
    /// <summary>
    /// Gets or sets the unique user ID.
    /// </summary>
    [JsonProperty("userid")]
    public string UserId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the URL to the user's icon/avatar.
    /// </summary>
    [JsonProperty("usericon")]
    public string UserIcon { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user account type (e.g., "regular", "premium").
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;
}
