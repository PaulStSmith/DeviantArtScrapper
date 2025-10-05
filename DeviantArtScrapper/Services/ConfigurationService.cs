using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using DeviantArtScrapper.Models;
using Newtonsoft.Json;

namespace DeviantArtScrapper.Services;

#pragma warning disable CA1822 // Mark members as static

/// <summary>
/// Provides secure storage and retrieval of API configuration using Windows Data Protection API (DPAPI).
/// </summary>
/// <remarks>
/// Configuration is encrypted using DPAPI and stored in a local file.
/// The encryption is user-specific, meaning only the current Windows user can decrypt the data.
/// </remarks>
[SupportedOSPlatform("windows")]
public class ConfigurationService
{
    private const string ConfigFileName = "config.json";
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("DeviantArtScrapper2025");

    /// <summary>
    /// Loads the API configuration from encrypted storage.
    /// </summary>
    /// <returns>A task containing the loaded configuration, or a new empty configuration if none exists or loading fails.</returns>
    /// <remarks>
    /// If the configuration file doesn't exist or cannot be decrypted, returns a new empty configuration.
    /// This method never throws exceptions.
    /// </remarks>
    public async Task<ApiConfiguration> LoadConfigurationAsync()
    {
        try
        {
            if (!File.Exists(ConfigFileName))
                return new ApiConfiguration();

            var encryptedData = await File.ReadAllBytesAsync(ConfigFileName);
            var decryptedData = ProtectedData.Unprotect(encryptedData, Entropy, DataProtectionScope.CurrentUser);
            var json = Encoding.UTF8.GetString(decryptedData);
            
            return JsonConvert.DeserializeObject<ApiConfiguration>(json) ?? new ApiConfiguration();
        }
        catch
        {
            return new ApiConfiguration();
        }
    }

    /// <summary>
    /// Saves the API configuration to encrypted storage.
    /// </summary>
    /// <param name="config">The configuration to save.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the configuration cannot be saved.</exception>
    /// <remarks>
    /// The configuration is serialized to JSON, encrypted using DPAPI, and saved to a local file.
    /// Only the current Windows user can decrypt and read the saved configuration.
    /// </remarks>
    public async Task SaveConfigurationAsync(ApiConfiguration config)
    {
        try
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            var data = Encoding.UTF8.GetBytes(json);
            var encryptedData = ProtectedData.Protect(data, Entropy, DataProtectionScope.CurrentUser);
            
            await File.WriteAllBytesAsync(ConfigFileName, encryptedData);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to save configuration", ex);
        }
    }
}