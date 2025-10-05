using System.Runtime.Versioning;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents the API configuration settings for DeviantArt authentication.
/// </summary>
[SupportedOSPlatform("windows")]
public class ApiConfiguration
{
    /// <summary>
    /// Gets or sets the DeviantArt OAuth2 client ID.
    /// </summary>
    /// <remarks>
    /// This is obtained from the DeviantArt developer portal when registering an application.
    /// </remarks>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the DeviantArt OAuth2 client secret.
    /// </summary>
    /// <remarks>
    /// This is obtained from the DeviantArt developer portal when registering an application.
    /// Keep this value secure and never share it publicly.
    /// </remarks>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the OAuth2 bearer token for API authentication.
    /// </summary>
    /// <remarks>
    /// This token is automatically obtained through client credentials flow.
    /// It expires after a certain period and needs to be refreshed.
    /// </remarks>
    public string? BearerToken { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time when the bearer token expires.
    /// </summary>
    /// <remarks>
    /// Used to determine when a new token needs to be requested.
    /// Tokens typically expire after 3600 seconds (1 hour).
    /// </remarks>
    public DateTime? TokenExpiration { get; set; }
}