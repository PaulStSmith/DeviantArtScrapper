using DeviantArtScrapper.Models;
using DeviantArtScrapper.Services;
using DeviantArtScrapper.Localization;
using System.Runtime.Versioning;

namespace DeviantArtScrapper.Forms;

#pragma warning disable IDE1006 // Naming Styles

/// <summary>
/// Settings form for configuring DeviantArt API credentials.
/// Provides UI for entering API credentials and testing the connection.
/// </summary>
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
    /// Updates the token status display label based on the current bearer token state.
    /// </summary>
    /// <remarks>
    /// Shows token expiration time if a valid token exists, otherwise displays
    /// "No token available" message.
    /// </remarks>
    private void UpdateTokenStatusDisplay()
    {
        if (!string.IsNullOrEmpty(Configuration.BearerToken) &&
            Configuration.TokenExpiration > DateTime.UtcNow)
        {
            lblTokenStatus.Text = Localizer.GetSettingsTokenValid(Configuration.TokenExpiration.Value);
        }
        else
        {
            lblTokenStatus.Text = Localizer.SettingsNoToken;
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
            MessageBox.Show(Localizer.SettingsEnterCredentials, Localizer.TitleValidationError,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnTestConnection.Enabled = false;
        btnTestConnection.Text = "Testing...";

        try
        {
            var apiService = new DeviantArtApiService();

            var authResponse = await apiService.AuthenticateAsync(txtClientId.Text.Trim(), txtClientSecret.Text.Trim());

            if (!apiService.IsSuccessful || string.IsNullOrEmpty(authResponse.AccessToken))
            {
                var errorMsg = apiService.LastException?.Message ?? "Authentication failed";
                MessageBox.Show(Localizer.GetSettingsConnectionFailed(errorMsg), Localizer.TitleError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var placeboResponse = await apiService.TestConnectionAsync(authResponse.AccessToken);

            if (apiService.IsSuccessful && placeboResponse.Status == "success")
            {
                Configuration.BearerToken = authResponse.AccessToken;
                Configuration.TokenExpiration = DateTime.UtcNow.AddSeconds(authResponse.ExpiresIn);

                UpdateTokenStatusDisplay();

                MessageBox.Show(Localizer.SettingsConnectionSuccess, Localizer.TitleSuccess,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var errorMsg = apiService.LastException?.Message ?? "Placebo test failed";
                MessageBox.Show(Localizer.GetSettingsConnectionFailed(errorMsg), Localizer.TitleError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(Localizer.GetSettingsConnectionFailed(ex.Message), Localizer.TitleError,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnTestConnection.Enabled = true;
            btnTestConnection.Text = Localizer.ButtonTestConnection;
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

    private void lblClientSecret_Click(object sender, EventArgs e)
    {

    }
}