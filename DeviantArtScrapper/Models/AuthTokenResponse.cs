using Newtonsoft.Json;

namespace DeviantArtScrapper.Models;

/// <summary>
/// Represents the response from DeviantArt OAuth2 token endpoint.
/// </summary>
public class AuthTokenResponse
{
    /// <summary>
    /// Gets or sets the OAuth2 access token.
    /// </summary>
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the token type (typically "Bearer").
    /// </summary>
    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the number of seconds until the token expires.
    /// </summary>
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    
    /// <summary>
    /// Gets or sets the status of the authentication request.
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}
