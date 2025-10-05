using DeviantArtScrapper.Models;
using DeviantArtScrapper.Services;
using System.Runtime.Versioning;

namespace DeviantArtScrapper.Forms;

#pragma warning disable IDE1006 // Naming Styles
/// <summary>
/// Represents a dialog form for configuring DeviantArt API settings.
/// </summary>
/// <remarks>
/// This form allows users to enter and test their DeviantArt API credentials,
/// including Client ID and Client Secret. It validates the credentials and
/// manages bearer token lifecycle.
/// </remarks>
[SupportedOSPlatform("windows")]
public partial class SettingsForm : Form
{
    /// <summary>
    /// Gets the API configuration containing client credentials and token information.
    /// </summary>
    public ApiConfiguration Configuration { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsForm"/> class.
    /// </summary>
    /// <param name="currentConfig">The current API configuration to load into the form.</param>
    public SettingsForm(ApiConfiguration currentConfig)
    {
        InitializeComponent();
        Configuration = new ApiConfiguration
        {
            ClientId = currentConfig.ClientId,
            ClientSecret = currentConfig.ClientSecret,
            BearerToken = currentConfig.BearerToken,
            TokenExpiration = currentConfig.TokenExpiration
        };
        
        LoadCurrentSettings();
        btnShowPassword.Click += btnShowPassword_Click;
    }

    /// <summary>
    /// Loads the current configuration settings into the form controls.
    /// </summary>
    private void LoadCurrentSettings()
    {
        txtClientId.Text = Configuration.ClientId;
        txtClientSecret.Text = Configuration.ClientSecret;
        UpdateTokenStatusDisplay();
    }

    /// <summary>
    /// Updates the token status label to reflect the current bearer token state.
    /// </summary>
    /// <remarks>
    /// Displays whether the token is valid, expired, or not available, along with
    /// the expiration date if the token is still valid.
    /// </remarks>
    private void UpdateTokenStatusDisplay()
    {
        if (!string.IsNullOrEmpty(Configuration.BearerToken))
        {
            lblTokenStatus.Text = Configuration.TokenExpiration > DateTime.UtcNow 
                ? $"Token valid until: {Configuration.TokenExpiration:yyyy-MM-dd HH:mm}"
                : "Token expired";
        }
        else
        {
            lblTokenStatus.Text = "No token available";
        }
    }

    /// <summary>
    /// Handles the Save button click event, validating and saving the configuration.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    /// <remarks>
    /// Validates that both Client ID and Client Secret are provided. If credentials
    /// have changed, the bearer token is cleared to force re-authentication.
    /// </remarks>
    private void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtClientId.Text))
        {
            MessageBox.Show("Client ID is required.", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtClientId.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(txtClientSecret.Text))
        {
            MessageBox.Show("Client Secret is required.", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtClientSecret.Focus();
            return;
        }

        Configuration.ClientId = txtClientId.Text.Trim();
        Configuration.ClientSecret = txtClientSecret.Text.Trim();
        
        // Clear token if credentials changed
        var credentialsChanged = Configuration.ClientId != txtClientId.Text.Trim() ||
                               Configuration.ClientSecret != txtClientSecret.Text.Trim();
        
        if (credentialsChanged)
        {
            Configuration.BearerToken = null;
            Configuration.TokenExpiration = null;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    /// <summary>
    /// Handles the Cancel button click event, closing the form without saving changes.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>
    /// Handles the Test Connection button click event, validating API credentials.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    /// <remarks>
    /// Authenticates with the DeviantArt API using the provided credentials and
    /// tests the connection using a placebo endpoint. If successful, updates the
    /// configuration with the new bearer token and expiration time.
    /// </remarks>
    private async void btnTestConnection_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtClientId.Text) || string.IsNullOrWhiteSpace(txtClientSecret.Text))
        {
            MessageBox.Show("Please enter both Client ID and Client Secret before testing.", "Missing Information",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Disable button and show progress
        btnTestConnection.Enabled = false;
        btnTestConnection.Text = "Testing...";
        
        try
        {
            var apiService = new DeviantArtApiService();
            
            // Step 1: Authenticate and get token
            var authResponse = await apiService.AuthenticateAsync(txtClientId.Text.Trim(), txtClientSecret.Text.Trim());
            
            if (!apiService.IsSuccessful || string.IsNullOrEmpty(authResponse.AccessToken))
            {
                var errorMsg = apiService.LastException?.Message ?? "Authentication failed";
                MessageBox.Show($"Authentication failed: {errorMsg}", "Connection Test Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 2: Test connection with placebo endpoint
            var placeboResponse = await apiService.TestConnectionAsync(authResponse.AccessToken);
            
            if (apiService.IsSuccessful && placeboResponse.Status == "success")
            {
                // Update configuration with new token
                Configuration.BearerToken = authResponse.AccessToken;
                Configuration.TokenExpiration = DateTime.UtcNow.AddSeconds(authResponse.ExpiresIn);
                
                // Update only the token status display, preserve user input
                UpdateTokenStatusDisplay();
                
                MessageBox.Show("Connection test successful! API credentials are valid.", "Connection Test Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var errorMsg = apiService.LastException?.Message ?? "Placebo test failed";
                MessageBox.Show($"Connection test failed: {errorMsg}", "Connection Test Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Connection test failed with error: {ex.Message}", "Connection Test Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            // Re-enable button
            btnTestConnection.Enabled = true;
            btnTestConnection.Text = "Test Connection";
        }
    }

    /// <summary>
    /// Handles the Show Password button click event, toggling client secret visibility.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    /// <remarks>
    /// Toggles between showing and hiding the client secret text, updating the button
    /// icon to reflect the current state (locked/unlocked).
    /// </remarks>
    private void btnShowPassword_Click(object? sender, EventArgs e)
    {
        if (txtClientSecret.PasswordChar == '•')
        {
            // Show password
            txtClientSecret.PasswordChar = '\0';
            btnShowPassword.Text = "🔓";
        }
        else
        {
            // Hide password
            txtClientSecret.PasswordChar = '•';
            btnShowPassword.Text = "🔒";
        }
    }
}