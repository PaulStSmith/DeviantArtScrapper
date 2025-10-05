using DeviantArtScrapper.Models;

namespace DeviantArtScrapper.Services;

/// <summary>
/// Defines the contract for interacting with the DeviantArt API.
/// </summary>
public interface IDeviantArtApi
{
    /// <summary>
    /// Authenticates with the DeviantArt API using client credentials.
    /// </summary>
    /// <param name="clientId">The OAuth2 client ID.</param>
    /// <param name="clientSecret">The OAuth2 client secret.</param>
    /// <returns>A task that represents the asynchronous authentication operation, containing the token response.</returns>
    Task<AuthTokenResponse> AuthenticateAsync(string clientId, string clientSecret);
    
    /// <summary>
    /// Tests the API connection using the placebo endpoint.
    /// </summary>
    /// <param name="bearerToken">The OAuth2 bearer token to test.</param>
    /// <returns>A task that represents the asynchronous test operation, containing the placebo response.</returns>
    Task<PlaceboResponse> TestConnectionAsync(string bearerToken);
    
    /// <summary>
    /// Gets the last exception that occurred during an API operation, if any.
    /// </summary>
    Exception? LastException { get; }
    
    /// <summary>
    /// Gets a value indicating whether the last API operation was successful.
    /// </summary>
    bool IsSuccessful { get; }
}