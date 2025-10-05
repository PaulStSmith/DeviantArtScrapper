using System.Net;
using System.Runtime.Versioning;
using System.Text;
using DeviantArtScrapper.Models;
using Newtonsoft.Json;
using RestSharp;

namespace DeviantArtScrapper.Services;

/// <summary>
/// Provides methods for interacting with the DeviantArt API including authentication,
/// gallery retrieval, and favorites collection.
/// </summary>
[SupportedOSPlatform("windows")]
public class DeviantArtApiService : IDeviantArtApi
{
    private const string BaseUrl = "https://www.deviantart.com";
    private string? _currentBearerToken;

    /// <summary>
    /// Gets the last exception that occurred during an API operation.
    /// </summary>
    public Exception? LastException { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the last API operation was successful.
    /// </summary>
    public bool IsSuccessful { get; private set; }

    /// <summary>
    /// Authenticates with DeviantArt using OAuth2 client credentials flow.
    /// </summary>
    /// <param name="clientId">The OAuth2 client ID from DeviantArt developer portal.</param>
    /// <param name="clientSecret">The OAuth2 client secret from DeviantArt developer portal.</param>
    /// <returns>A task containing the authentication token response.</returns>
    /// <exception cref="InvalidOperationException">Thrown when authentication fails.</exception>
    public async Task<AuthTokenResponse> AuthenticateAsync(string clientId, string clientSecret)
    {
        LastException = null;
        IsSuccessful = false;

        try
        {
            using var client = new RestClient(BaseUrl);
            var request = new RestRequest("/oauth2/token", Method.Post);
            
            // Basic authentication using client credentials
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            request.AddHeader("Authorization", $"Basic {credentials}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            
            // Add form data
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);

            var response = await client.ExecuteAsync(request);
            
            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var tokenResponse = JsonConvert.DeserializeObject<AuthTokenResponse>(response.Content);
                if (tokenResponse != null)
                {
                    IsSuccessful = true;
                    return tokenResponse;
                }
            }

            // Handle error response
            await HandleErrorResponse(response);
            return new AuthTokenResponse();
        }
        catch (Exception ex)
        {
            LastException = ex;
            IsSuccessful = false;
            throw new InvalidOperationException("Failed to authenticate with DeviantArt API", ex);
        }
    }

    /// <summary>
    /// Tests the API connection using the DeviantArt placebo endpoint.
    /// </summary>
    /// <param name="bearerToken">The bearer token to validate.</param>
    /// <returns>A task containing the placebo response.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the connection test fails.</exception>
    public async Task<PlaceboResponse> TestConnectionAsync(string bearerToken)
    {
        LastException = null;
        IsSuccessful = false;

        try
        {
            using var client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/v1/oauth2/placebo", Method.Get);
            
            // Bearer token authentication
            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            request.AddHeader("Accept", "application/json");

            var response = await client.ExecuteAsync(request);
            
            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var placeboResponse = JsonConvert.DeserializeObject<PlaceboResponse>(response.Content);
                if (placeboResponse != null)
                {
                    IsSuccessful = true;
                    return placeboResponse;
                }
            }

            // Handle error response
            await HandleErrorResponse(response);
            return new PlaceboResponse();
        }
        catch (Exception ex)
        {
            LastException = ex;
            IsSuccessful = false;
            throw new InvalidOperationException("Failed to test connection with DeviantArt API", ex);
        }
    }

    /// <summary>
    /// Initializes the API service with configuration and ensures a valid bearer token is available.
    /// </summary>
    /// <param name="config">The API configuration containing credentials and token information.</param>
    /// <returns>A task representing the asynchronous initialization operation.</returns>
    /// <remarks>
    /// If the token is missing or expired, this method will automatically authenticate to obtain a new token.
    /// </remarks>
    public async Task InitializeAsync(ApiConfiguration config)
    {
        if (string.IsNullOrEmpty(config.BearerToken) || config.TokenExpiration <= DateTime.UtcNow)
        {
            var tokenResponse = await AuthenticateAsync(config.ClientId, config.ClientSecret);
            config.BearerToken = tokenResponse.AccessToken;
            config.TokenExpiration = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
        }
        _currentBearerToken = config.BearerToken;
    }

    /// <summary>
    /// Retrieves a page of gallery deviations for the specified user.
    /// </summary>
    /// <param name="username">The DeviantArt username whose gallery to retrieve.</param>
    /// <param name="limit">The maximum number of deviations to retrieve (default: 20, max: 24).</param>
    /// <param name="offset">The offset for pagination (default: 0).</param>
    /// <returns>A task containing the gallery response with deviations and pagination info, or null if the request fails.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the gallery retrieval fails after all attempts.</exception>
    public async Task<GalleryResponse?> GetGalleryAsync(string username, int limit = 20, int offset = 0)
    {
        LastException = null;
        IsSuccessful = false;

        try
        {
            using var client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/v1/oauth2/gallery/all", Method.Get);
            
            // Bearer token authentication
            if (!string.IsNullOrEmpty(_currentBearerToken))
                request.AddHeader("Authorization", $"Bearer {_currentBearerToken}");
            
            request.AddHeader("Accept", "application/json");
            request.AddParameter("username", username);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            var response = await client.ExecuteAsync(request);
            
            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var galleryResponse = JsonConvert.DeserializeObject<GalleryResponse>(response.Content);
                if (galleryResponse != null)
                {
                    IsSuccessful = true;
                    return galleryResponse;
                }
            }

            // Handle error response
            await HandleErrorResponse(response);
            return null;
        }
        catch (Exception ex)
        {
            LastException = ex;
            IsSuccessful = false;
            throw new InvalidOperationException($"Failed to get gallery for user '{username}'", ex);
        }
    }

    /// <summary>
    /// Retrieves a page of favorited deviations (collections) for the specified user.
    /// </summary>
    /// <param name="username">The DeviantArt username whose favorites to retrieve.</param>
    /// <param name="limit">The maximum number of favorites to retrieve (default: 20, max: 24).</param>
    /// <param name="offset">The offset for pagination (default: 0).</param>
    /// <returns>A task containing the collection response with favorited deviations and pagination info, or null if the request fails.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the favorites retrieval fails after all attempts.</exception>
    public async Task<CollectionResponse?> GetCollectionsAsync(string username, int limit = 20, int offset = 0)
    {
        LastException = null;
        IsSuccessful = false;

        try
        {
            using var client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/v1/oauth2/collections/all", Method.Get);
            
            // Bearer token authentication
            if (!string.IsNullOrEmpty(_currentBearerToken))
                request.AddHeader("Authorization", $"Bearer {_currentBearerToken}");
            
            request.AddHeader("Accept", "application/json");
            request.AddParameter("username", username);
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            var response = await client.ExecuteAsync(request);
            
            if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(response.Content))
            {
                var collectionResponse = JsonConvert.DeserializeObject<CollectionResponse>(response.Content);
                if (collectionResponse != null)
                {
                    IsSuccessful = true;
                    return collectionResponse;
                }
            }

            // Handle error response
            await HandleErrorResponse(response);
            return null;
        }
        catch (Exception ex)
        {
            LastException = ex;
            IsSuccessful = false;
            throw new InvalidOperationException($"Failed to get collections for user '{username}'", ex);
        }
    }

    /// <summary>
    /// Handles error responses from the DeviantArt API by parsing error information and storing it.
    /// </summary>
    /// <param name="response">The REST response that contains the error.</param>
    /// <returns>A task representing the asynchronous error handling operation.</returns>
    private async Task HandleErrorResponse(RestResponse response)
    {
        var errorMessage = $"API call failed with status {(int)response.StatusCode} ({response.StatusCode})";
        
        if (!string.IsNullOrEmpty(response.Content))
        {
            try
            {
                var error = JsonConvert.DeserializeObject<DeviantArtError>(response.Content);
                if (error != null && !string.IsNullOrEmpty(error.Error))
                {
                    errorMessage = $"{error.Error}: {error.ErrorDescription}";
                }
            }
            catch
            {
                // If we can't parse the error, use the raw content
                errorMessage = $"{errorMessage}. Response: {response.Content}";
            }
        }

        LastException = new HttpRequestException(errorMessage)
        {
            Data = {
                ["StatusCode"] = response.StatusCode,
                ["Content"] = response.Content ?? "",
                ["ErrorMessage"] = response.ErrorMessage ?? ""
            }
        };
        
        await Task.CompletedTask; // Make method async for consistency
    }
}