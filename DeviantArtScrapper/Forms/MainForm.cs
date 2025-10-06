using DeviantArtScrapper.Models;
using DeviantArtScrapper.Services;
using DeviantArtScrapper.Localization;
using System.Runtime.Versioning;
using System.Text;

namespace DeviantArtScrapper.Forms;

#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE1006 // Naming Styles

/// <summary>
/// Main application form for the DeviantArt Scrapper application.
/// Provides UI for scraping gallery items and favorites from DeviantArt users.
/// </summary>
[SupportedOSPlatform("windows")]
public partial class MainForm : Form
{
    private readonly ConfigurationService _configService;
    private readonly DeviantArtApiService _apiService;
    private ApiConfiguration _currentConfig;
    private bool _isScrapingInProgress;
    private CancellationTokenSource? _cancellationTokenSource;

    // Retry configuration
    private const int MaxRetryAttempts = 3;
    private const int InitialRetryDelayMs = 1000;
    
    // Rate limiting configuration
    private const int MinDelayBetweenRequestsMs = 500;
    private DateTime _lastRequestTime = DateTime.MinValue;

    // Transfer rate tracking
    private DateTime _scrapingStartTime;
    private int _lastItemCount;
    private DateTime _lastUpdateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainForm"/> class.
    /// Sets up services and loads configuration asynchronously.
    /// </summary>
    public MainForm()
    {
        InitializeComponent();
        _configService = new ConfigurationService();
        _apiService = new DeviantArtApiService();
        _currentConfig = new ApiConfiguration();
        _isScrapingInProgress = false;

        _ = LoadConfigurationAsync();
    }

    /// <summary>
    /// Loads the API configuration from encrypted storage and updates the UI status.
    /// </summary>
    /// <returns>A task representing the asynchronous load operation.</returns>
    private async Task LoadConfigurationAsync()
    {
        try
        {
            _currentConfig = await _configService.LoadConfigurationAsync();
            UpdateStatusLabel();
        }
        catch (Exception ex)
        {
            MessageBox.Show(Localizer.GetSettingsLoadFailed(ex.Message), Localizer.TitleError,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Updates the status label to reflect the current API configuration and authentication state.
    /// </summary>
    private void UpdateStatusLabel()
    {
        var hasCredentials = !string.IsNullOrEmpty(_currentConfig.ClientId) &&
                             !string.IsNullOrEmpty(_currentConfig.ClientSecret);

        var hasToken = !string.IsNullOrEmpty(_currentConfig.BearerToken) &&
                       _currentConfig.TokenExpiration > DateTime.UtcNow;

        if (hasCredentials && hasToken)
            lblStatus.Text = Localizer.StatusReady;
        else if (hasCredentials)
            lblStatus.Text = Localizer.StatusConfigured;
        else
            lblStatus.Text = Localizer.StatusNotConfigured;
    }

    /// <summary>
    /// Handles the Settings button click event.
    /// Opens the settings dialog and saves any configuration changes.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private async void btnSettings_Click(object sender, EventArgs e)
    {
        using var settingsForm = new SettingsForm(_currentConfig);
        if (settingsForm.ShowDialog() == DialogResult.OK)
        {
            _currentConfig = settingsForm.Configuration;
            try
            {
                await _configService.SaveConfigurationAsync(_currentConfig);
                UpdateStatusLabel();
                MessageBox.Show(Localizer.SettingsSavedSuccess, Localizer.TitleSuccess,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Localizer.GetSettingsSaveFailed(ex.Message), Localizer.TitleError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    /// <summary>
    /// Handles the Browse button click event for gallery export.
    /// Opens a file save dialog to select the export file location.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void btnBrowse_Click(object sender, EventArgs e)
    {
        using var saveDialog = new SaveFileDialog
        {
            Filter = GetFileFilter(),
            DefaultExt = GetFileExtension(),
            FileName = txtFileName.Text,
            AddExtension = true
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            // Remove extension from the saved path since we add it in export methods
            var fileNameWithoutExt = saveDialog.FileName;
            txtFileName.Text = fileNameWithoutExt;
        }
    }

    /// <summary>
    /// Removes common export file extensions from the given filename if present.
    /// </summary>
    /// <param name="fileName">The filename to process.</param>
    /// <returns>The filename without extension, or the original filename if no known extension is present.</returns>
    private string RemoveExtensionIfPresent(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return fileName;

        // Remove common extensions if present
        var extensions = new[] { ".csv", ".html", ".xlsx" };
        foreach (var ext in extensions)
        {
            if (fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                return fileName[..^ext.Length];
        }

        return fileName;
    }

    /// <summary>
    /// Handles the Start Scraping button click event for gallery scraping.
    /// Validates inputs and initiates the scraping process.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private async void btnStartScraping_Click(object sender, EventArgs e)
    {
        if (_isScrapingInProgress)
        {
            MessageBox.Show(Localizer.MessageScrapingInProgress, Localizer.TitleInformation,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!ValidateInputs())
            return;

        await StartScrapingAsync();
    }

    /// <summary>
    /// Handles the Cancel Scraping button click event.
    /// Requests cancellation of the current scraping operation.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void btnCancelScraping_Click(object sender, EventArgs e)
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            UpdateProgressLabel(Localizer.ProgressCancelling);
        }
    }

    /// <summary>
    /// Validates the user inputs for gallery scraping.
    /// Checks for required username, filename, and API credentials.
    /// </summary>
    /// <returns><c>true</c> if all inputs are valid; otherwise, <c>false</c>.</returns>
    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(txtUsername.Text))
        {
            MessageBox.Show(Localizer.MessageEnterUsername, Localizer.TitleValidationError,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUsername.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtFileName.Text))
        {
            MessageBox.Show(Localizer.MessageEnterFilename, Localizer.TitleValidationError,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtFileName.Focus();
            return false;
        }

        var hasCredentials = !string.IsNullOrEmpty(_currentConfig.ClientId) &&
                           !string.IsNullOrEmpty(_currentConfig.ClientSecret);

        if (!hasCredentials)
        {
            MessageBox.Show(Localizer.MessageConfigureApi, Localizer.TitleConfigRequired,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Initiates and manages the gallery scraping process.
    /// Fetches all gallery items, handles cancellation, and exports the results.
    /// </summary>
    /// <returns>A task representing the asynchronous scraping operation.</returns>
    private async Task StartScrapingAsync()
    {
        _isScrapingInProgress = true;
        _cancellationTokenSource = new CancellationTokenSource();
        SetProgressState(ProgressState.Running);

        // Initialize transfer rate tracking
        _scrapingStartTime = DateTime.UtcNow;
        _lastItemCount = 0;
        _lastUpdateTime = _scrapingStartTime;

        var allDeviations = new List<Deviation>();

        try
        {
            var username = txtUsername.Text.Trim();
            var fileName = txtFileName.Text.Trim();

            await _apiService.InitializeAsync(_currentConfig);

            var offset = 0;
            const int limit = 20;
            var hasMore = true;

            while (hasMore && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                // Apply rate limiting
                await ApplyRateLimitingAsync(_cancellationTokenSource.Token);

                // Fetch gallery with retry logic
                var response = await FetchGalleryWithRetryAsync(username, limit, offset, _cancellationTokenSource.Token);

                if (response?.Results != null)
                {
                    allDeviations.AddRange(response.Results);
                    hasMore = response.HasMore;
                    offset = response.NextOffset ?? offset + limit;

                    // Update progress and transfer rate
                    UpdateProgressLabel(Localizer.GetProgressCollected(allDeviations.Count));
                    UpdateTransferRate(allDeviations.Count);
                }
                else
                {
                    hasMore = false;
                }
            }

            // Check if cancelled
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                await HandleCancellationAsync(allDeviations, fileName, username);
                return;
            }

            if (allDeviations.Count == 0)
            {
                MessageBox.Show(Localizer.GetMessageNoGalleryItems(username), Localizer.TitleNoResults,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Sort by date (newest first) - single source of sorting
            allDeviations = [.. allDeviations.OrderByDescending(d => d.PublishedTime ?? DateTime.MinValue)];

            UpdateProgressLabel(Localizer.ProgressExporting);
            await ExportDataAsync(allDeviations, fileName, username);

            string completionMessage;
            if (rbHTML.Checked)
            {
                var pageCount = (int)Math.Ceiling(allDeviations.Count / 100.0);
                completionMessage = Localizer.GetMessageSuccessScrapedHtml(allDeviations.Count, pageCount);
            }
            else if (rbXLSX.Checked)
            {
                completionMessage = Localizer.GetMessageSuccessScraped(allDeviations.Count, fileName);
            }
            else
            {
                completionMessage = Localizer.GetMessageSuccessScraped(allDeviations.Count, fileName);
            }

            SetProgressState(ProgressState.Success);
            MessageBox.Show(completionMessage, Localizer.TitleExportComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (OperationCanceledException)
        {
            await HandleCancellationAsync(allDeviations, txtFileName.Text.Trim(), txtUsername.Text.Trim());
        }
        catch (Exception ex)
        {
            SetProgressState(ProgressState.Error);
            
            // If cancelled during error, still offer partial export
            if (_cancellationTokenSource?.Token.IsCancellationRequested == true && allDeviations.Count > 0)
            {
                await HandleCancellationAsync(allDeviations, txtFileName.Text.Trim(), txtUsername.Text.Trim());
            }
            else
            {
                MessageBox.Show(Localizer.GetMessageScrapingError(ex.Message), Localizer.TitleError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            _isScrapingInProgress = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            SetProgressState(ProgressState.Idle);
        }
    }

    /// <summary>
    /// Handles the cancellation of a scraping operation.
    /// Offers to export partial results if any items were collected.
    /// </summary>
    private async Task HandleCancellationAsync(List<Deviation> collectedDeviations, string fileName, string username)
    {
        SetProgressState(ProgressState.Idle);

        if (collectedDeviations.Count > 0)
        {
            var result = MessageBox.Show(
                Localizer.GetMessageCancelledPartial(collectedDeviations.Count),
                Localizer.TitlePartialExport,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    UpdateProgressLabel(Localizer.ProgressExporting);
                    await ExportDataAsync(collectedDeviations, fileName, username);

                    string exportMessage;
                    if (rbHTML.Checked)
                    {
                        var pageCount = (int)Math.Ceiling(collectedDeviations.Count / 100.0);
                        exportMessage = Localizer.GetMessagePartialExportedHtml(pageCount);
                    }
                    else if (rbXLSX.Checked)
                    {
                        exportMessage = Localizer.GetMessagePartialExportedFile($"{fileName}.xlsx");
                    }
                    else
                    {
                        exportMessage = Localizer.GetMessagePartialExportedFile($"{fileName}.csv");
                    }

                    MessageBox.Show(
                        Localizer.GetMessagePartialExported(collectedDeviations.Count, exportMessage),
                        Localizer.TitlePartialComplete,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        Localizer.GetMessagePartialExportError(ex.Message),
                        Localizer.TitleExportError,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show(
                Localizer.MessageCancelledNoItems,
                Localizer.TitleCancelled,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// Exports the collected deviations to the selected file format.
    /// </summary>
    /// <param name="deviations">The list of deviations to export.</param>
    /// <param name="fileName">The target filename without extension.</param>
    /// <param name="username">The DeviantArt username for HTML export metadata.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no export format is selected.</exception>
    private async Task ExportDataAsync(List<Deviation> deviations, string fileName, string username)
    {
        // Ensure fileName doesn't have extension (we add it in each export method)
        fileName = RemoveExtensionIfPresent(fileName);

        if (rbCSV.Checked)
        {
            await ExportToCsvAsync(deviations, fileName);
        }
        else if (rbHTML.Checked)
        {
            await ExportToHtmlAsync(deviations, fileName, username);
        }
        else if (rbXLSX.Checked)
        {
            await ExportToXlsxAsync(deviations, fileName);
        }
        else
        {
            throw new InvalidOperationException(Localizer.ExceptionNotImplemented);
        }
    }

    /// <summary>
    /// Fetches a page of gallery items with automatic retry logic on failure.
    /// Implements exponential backoff for retry attempts.
    /// </summary>
    /// <param name="username">The DeviantArt username whose gallery to fetch.</param>
    /// <param name="limit">The maximum number of items to fetch per request.</param>
    /// <param name="offset">The pagination offset.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task containing the gallery response, or null if all retry attempts fail.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    /// <exception cref="InvalidOperationException">Thrown when all retry attempts are exhausted.</exception>
    private async Task<GalleryResponse?> FetchGalleryWithRetryAsync(
        string username,
        int limit,
        int offset,
        CancellationToken cancellationToken)
    {
        var attempt = 0;
        var delayMs = InitialRetryDelayMs;

        while (attempt < MaxRetryAttempts)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var response = await _apiService.GetGalleryAsync(username, limit, offset);
                _lastRequestTime = DateTime.UtcNow;
                return response;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                attempt++;
                
                if (attempt >= MaxRetryAttempts)
                {
                    throw new InvalidOperationException(
                        $"Failed to fetch gallery after {MaxRetryAttempts} attempts", ex);
                }

                // Exponential backoff
                UpdateProgressLabel(Localizer.GetProgressRequestFailed(attempt, MaxRetryAttempts, delayMs));
                await Task.Delay(delayMs, cancellationToken);
                delayMs *= 2; // Exponential backoff
            }
        }

        return null;
    }

    /// <summary>
    /// Applies rate limiting between API requests to avoid overwhelming the server.
    /// Ensures a minimum delay between consecutive requests.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the delay operation.</param>
    /// <returns>A task representing the asynchronous delay operation.</returns>
    private async Task ApplyRateLimitingAsync(CancellationToken cancellationToken)
    {
        if (_lastRequestTime != DateTime.MinValue)
        {
            var timeSinceLastRequest = DateTime.UtcNow - _lastRequestTime;
            var remainingDelay = MinDelayBetweenRequestsMs - (int)timeSinceLastRequest.TotalMilliseconds;

            if (remainingDelay > 0)
            {
                await Task.Delay(remainingDelay, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Updates the progress label with the current status message.
    /// </summary>
    /// <param name="message">The status message to display.</param>
    private void UpdateProgressLabel(string message)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string>(UpdateProgressLabel), message);
            return;
        }

        lblProgress.Text = message;
    }

    /// <summary>
    /// Updates the transfer rate display based on items collected.
    /// </summary>
    /// <param name="currentItemCount">The current number of items collected.</param>
    private void UpdateTransferRate(int currentItemCount)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<int>(UpdateTransferRate), currentItemCount);
            return;
        }

        var now = DateTime.UtcNow;
        var elapsedSinceStart = (now - _scrapingStartTime).TotalSeconds;
        
        if (elapsedSinceStart > 0)
        {
            // Calculate items per second
            var itemsPerSecond = currentItemCount / elapsedSinceStart;
            
            // Estimate KB/s (assuming ~2KB per deviation metadata)
            var kbPerSecond = itemsPerSecond * 2;
            
            lblTransferRate.Text = $"{kbPerSecond:F1} KB/s";
            lblTransferRate.Visible = true;
        }
    }

    /// <summary>
    /// Updates the progress label with the current status message for favorites tab.
    /// </summary>
    /// <param name="message">The status message to display.</param>
    private void UpdateProgressLabelFav(string message)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string>(UpdateProgressLabelFav), message);
            return;
        }

        lblProgressFav.Text = message;
    }

    /// <summary>
    /// Updates the transfer rate display based on items collected for favorites tab.
    /// </summary>
    /// <param name="currentItemCount">The current number of items collected.</param>
    private void UpdateTransferRateFav(int currentItemCount)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<int>(UpdateTransferRateFav), currentItemCount);
            return;
        }

        var now = DateTime.UtcNow;
        var elapsedSinceStart = (now - _scrapingStartTime).TotalSeconds;
        
        if (elapsedSinceStart > 0)
        {
            // Calculate items per second
            var itemsPerSecond = currentItemCount / elapsedSinceStart;
            
            // Estimate KB/s (assuming ~2KB per deviation metadata)
            var kbPerSecond = itemsPerSecond * 2;
            
            lblTransferRateFav.Text = $"{kbPerSecond:F1} KB/s";
            lblTransferRateFav.Visible = true;
        }
    }

    /// <summary>
    /// Exports the collected deviations to a CSV file.
    /// </summary>
    /// <param name="deviations">The list of deviations to export.</param>
    /// <param name="fileName">The target filename without extension.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    private async Task ExportToCsvAsync(List<Deviation> deviations, string fileName)
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, $"{fileName}.csv");
        var csv = new StringBuilder();

        csv.AppendLine("Title,Author,URL,Published Date,Mature Content,Stats Favourites,Stats Comments,Download URL,Thumbnail URL");

        foreach (var deviation in deviations)
        {
            var publishedDate = deviation.PublishedTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
            var downloadUrl = deviation.Content?.Src ?? "";
            var thumbnailUrl = deviation.Thumbs?.FirstOrDefault()?.Src ?? "";

            csv.AppendLine($"\"{EscapeCsv(deviation.Title ?? "")}\"," +
                           $"\"{EscapeCsv(deviation.Author?.Username ?? "")}\"," +
                           $"\"{deviation.Url ?? ""}\"," +
                           $"\"{publishedDate}\"," +
                           $"{deviation.IsMature}," +
                           $"\"{deviation.Stats?.Favourites ?? 0}\"," +
                           $"\"{deviation.Stats?.Comments ?? 0}\"," +
                           $"\"{downloadUrl}\"," +
                           $"\"{thumbnailUrl}\"");
        }

        await File.WriteAllTextAsync(filePath, csv.ToString(), Encoding.UTF8);
    }

    /// <summary>
    /// Escapes special characters in CSV values to prevent formatting issues.
    /// </summary>
    /// <param name="value">The value to escape.</param>
    /// <returns>The escaped value safe for CSV export.</returns>
    private static string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        return value.Replace("\"", "\"\"");
    }

    /// <summary>
    /// Gets the file filter string for the save dialog based on the selected export format.
    /// </summary>
    /// <returns>A file filter string for use in file dialogs.</returns>
    private string GetFileFilter()
    {
        if (rbCSV.Checked)
            return Localizer.FilterCsv;
        if (rbHTML.Checked)
            return Localizer.FilterHtml;
        if (rbXLSX.Checked)
            return Localizer.FilterXlsx;

        return Localizer.FilterAll;
    }

    /// <summary>
    /// Gets the default file extension based on the selected export format.
    /// </summary>
    /// <returns>A file extension string including the leading dot.</returns>
    private string GetFileExtension()
    {
        if (rbCSV.Checked)
            return ".csv";
        if (rbHTML.Checked)
            return ".html";
        if (rbXLSX.Checked)
            return ".xlsx";

        return "";
    }

    /// <summary>
    /// Sets the visual state of the progress bar and related UI controls for the gallery tab.
    /// </summary>
    /// <param name="state">The desired progress state.</param>
    private void SetProgressState(ProgressState state)
    {
        switch (state)
        {
            case ProgressState.Idle:
                progressScraping.Visible = false;
                lblProgress.Visible = false;
                lblTransferRate.Visible = false;
                btnCancelScraping.Visible = false;
                btnStartScraping.Enabled = true;
                btnStartScraping.Text = Localizer.ButtonStartScraping;
                break;
            case ProgressState.Running:
                progressScraping.Visible = true;
                progressScraping.Style = ProgressBarStyle.Marquee;
                progressScraping.MarqueeAnimationSpeed = 20;
                lblProgress.Visible = true;
                lblProgress.Text = Localizer.ProgressInitializing;
                lblTransferRate.Visible = false;
                btnCancelScraping.Visible = true;
                btnStartScraping.Enabled = false;
                btnStartScraping.Text = Localizer.ProgressScraping;
                break;
            case ProgressState.Success:
                progressScraping.Style = ProgressBarStyle.Continuous;
                progressScraping.Value = 100;
                lblProgress.Text = Localizer.ProgressExportComplete;
                lblTransferRate.Visible = false;
                btnStartScraping.Text = Localizer.ProgressComplete;
                btnCancelScraping.Visible = false;
                break;
            case ProgressState.Error:
                progressScraping.Style = ProgressBarStyle.Continuous;
                progressScraping.Value = 0;
                lblProgress.Text = Localizer.ProgressError;
                lblTransferRate.Visible = false;
                btnStartScraping.Text = Localizer.TitleError;
                btnCancelScraping.Visible = false;
                break;
        }
    }

    /// <summary>
    /// Exports the collected deviations to HTML format with pagination.
    /// </summary>
    /// <param name="deviations">The list of deviations to export.</param>
    /// <param name="fileName">The target filename without extension.</param>
    /// <param name="username">The DeviantArt username for metadata.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    private async Task ExportToHtmlAsync(List<Deviation> deviations, string fileName, string username)
    {
        var htmlService = new HtmlExportService();
        await htmlService.ExportToHtmlAsync(deviations, fileName, username);
    }

    /// <summary>
    /// Exports the collected deviations to an Excel (XLSX) file.
    /// </summary>
    /// <param name="deviations">The list of deviations to export.</param>
    /// <param name="fileName">The target filename without extension.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    private async Task ExportToXlsxAsync(List<Deviation> deviations, string fileName)
    {
        var xlsxService = new XlsxExportService();
        await xlsxService.ExportToXlsxAsync(deviations, fileName);
    }

    // Favorites tab event handlers

    /// <summary>
    /// Handles the Browse button click event for favorites export.
    /// Opens a file save dialog to select the export file location.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void btnBrowseFav_Click(object sender, EventArgs e)
    {
        using var saveDialog = new SaveFileDialog
        {
            Filter = GetFileFilterFav(),
            DefaultExt = GetFileExtensionFav(),
            FileName = txtFileNameFav.Text,
            AddExtension = true
        };

        if (saveDialog.ShowDialog() == DialogResult.OK)
        {
            var fileNameWithoutExt = saveDialog.FileName;
            txtFileNameFav.Text = fileNameWithoutExt;
        }
    }

    /// <summary>
    /// Handles the Start Scraping button click event for favorites scraping.
    /// Validates inputs and initiates the favorites scraping process.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private async void btnStartScrapingFav_Click(object sender, EventArgs e)
    {
        if (_isScrapingInProgress)
        {
            MessageBox.Show(Localizer.MessageScrapingInProgress, Localizer.TitleInformation,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!ValidateInputsFav())
            return;

        await StartScrapingFavoritesAsync();
    }

    /// <summary>
    /// Handles the Cancel Scraping button click event for favorites.
    /// Requests cancellation of the current favorites scraping operation.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void btnCancelScrapingFav_Click(object sender, EventArgs e)
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
        {
            _cancellationTokenSource.Cancel();
            UpdateProgressLabelFav(Localizer.ProgressCancelling);
        }
    }

    /// <summary>
    /// Validates the user inputs for favorites scraping.
    /// Checks for required username, filename, and API credentials.
    /// </summary>
    /// <returns><c>true</c> if all inputs are valid; otherwise, <c>false</c>.</returns>
    private bool ValidateInputsFav()
    {
        if (string.IsNullOrWhiteSpace(txtUsernameFav.Text))
        {
            MessageBox.Show(Localizer.MessageEnterUsername, Localizer.TitleValidationError,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtUsernameFav.Focus();
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtFileNameFav.Text))
        {
            MessageBox.Show(Localizer.MessageEnterFilename, Localizer.TitleValidationError,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtFileNameFav.Focus();
            return false;
        }

        var hasCredentials = !string.IsNullOrEmpty(_currentConfig.ClientId) &&
                           !string.IsNullOrEmpty(_currentConfig.ClientSecret);

        if (!hasCredentials)
        {
            MessageBox.Show(Localizer.MessageConfigureApi, Localizer.TitleConfigRequired,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Initiates and manages the favorites scraping process.
    /// Fetches all favorited items, handles cancellation, and exports the results.
    /// </summary>
    /// <returns>A task representing the asynchronous scraping operation.</returns>
    private async Task StartScrapingFavoritesAsync()
    {
        _isScrapingInProgress = true;
        _cancellationTokenSource = new CancellationTokenSource();
        SetProgressStateFav(ProgressState.Running);

        // Initialize transfer rate tracking
        _scrapingStartTime = DateTime.UtcNow;
        _lastItemCount = 0;
        _lastUpdateTime = _scrapingStartTime;

        var allDeviations = new List<Deviation>();

        try
        {
            var username = txtUsernameFav.Text.Trim();
            var fileName = txtFileNameFav.Text.Trim();

            await _apiService.InitializeAsync(_currentConfig);

            var offset = 0;
            const int limit = 20;
            var hasMore = true;

            while (hasMore && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                await ApplyRateLimitingAsync(_cancellationTokenSource.Token);

                var response = await FetchCollectionsWithRetryAsync(username, limit, offset, _cancellationTokenSource.Token);

                if (response?.Results != null)
                {
                    allDeviations.AddRange(response.Results);
                    hasMore = response.HasMore;
                    offset = response.NextOffset ?? offset + limit;

                    UpdateProgressLabelFav(Localizer.GetProgressCollectedFavorites(allDeviations.Count));
                    UpdateTransferRateFav(allDeviations.Count);
                }
                else
                {
                    hasMore = false;
                }
            }

            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                await HandleCancellationFavAsync(allDeviations, fileName, username);
                return;
            }

            if (allDeviations.Count == 0)
            {
                MessageBox.Show(Localizer.GetMessageNoFavorites(username), Localizer.TitleNoResults,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            allDeviations = [.. allDeviations
                .OrderBy(d => d.Author?.Username ?? "")
                .ThenByDescending(d => d.PublishedTime ?? DateTime.MinValue)];

            UpdateProgressLabelFav(Localizer.ProgressExporting);
            await ExportDataFavAsync(allDeviations, fileName, username);

            string completionMessage;
            if (rbHTMLFav.Checked)
            {
                var pageCount = (int)Math.Ceiling(allDeviations.Count / 100.0);
                completionMessage = Localizer.GetMessageSuccessScrapedFavoritesHtml(allDeviations.Count, pageCount);
            }
            else if (rbXLSXFav.Checked)
            {
                completionMessage = Localizer.GetMessageSuccessScrapedFavorites(allDeviations.Count, $"{fileName}.xlsx");
            }
            else
            {
                completionMessage = Localizer.GetMessageSuccessScrapedFavorites(allDeviations.Count, $"{fileName}.csv");
            }

            SetProgressStateFav(ProgressState.Success);
            MessageBox.Show(completionMessage, Localizer.TitleExportComplete, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (OperationCanceledException)
        {
            await HandleCancellationFavAsync(allDeviations, txtFileNameFav.Text.Trim(), txtUsernameFav.Text.Trim());
        }
        catch (Exception ex)
        {
            SetProgressStateFav(ProgressState.Error);

            if (_cancellationTokenSource?.Token.IsCancellationRequested == true && allDeviations.Count > 0)
            {
                await HandleCancellationFavAsync(allDeviations, txtFileNameFav.Text.Trim(), txtUsernameFav.Text.Trim());
            }
            else
            {
                MessageBox.Show(Localizer.GetMessageScrapingError(ex.Message), Localizer.TitleError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            _isScrapingInProgress = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            SetProgressStateFav(ProgressState.Idle);
        }
    }

    /// <summary>
    /// Fetches a page of collection (favorites) items with automatic retry logic on failure.
    /// Implements exponential backoff for retry attempts.
    /// </summary>
    /// <param name="username">The DeviantArt username whose favorites to fetch.</param>
    /// <param name="limit">The maximum number of items to fetch per request.</param>
    /// <param name="offset">The pagination offset.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A task containing the collection response, or null if all retry attempts fail.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    /// <exception cref="InvalidOperationException">Thrown when all retry attempts are exhausted.</exception>
    private async Task<CollectionResponse?> FetchCollectionsWithRetryAsync(
        string username,
        int limit,
        int offset,
        CancellationToken cancellationToken)
    {
        var attempt = 0;
        var delayMs = InitialRetryDelayMs;

        while (attempt < MaxRetryAttempts)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var response = await _apiService.GetCollectionsAsync(username, limit, offset);
                _lastRequestTime = DateTime.UtcNow;
                return response;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                attempt++;

                if (attempt >= MaxRetryAttempts)
                {
                    throw new InvalidOperationException(
                        $"Failed to fetch favorites after {MaxRetryAttempts} attempts", ex);
                }

                UpdateProgressLabelFav(Localizer.GetProgressRequestFailed(attempt, MaxRetryAttempts, delayMs));
                await Task.Delay(delayMs, cancellationToken);
                delayMs *= 2;
            }
        }

        return null;
    }

    /// <summary>
    /// Handles the cancellation of a favorites scraping operation.
    /// Offers to export partial results if any favorites were collected.
    /// </summary>
    /// <param name="collectedDeviations">The list of favorited deviations collected before cancellation.</param>
    /// <param name="fileName">The target filename for export.</param>
    /// <param name="username">The DeviantArt username being scraped.</param>
    /// <returns>A task representing the asynchronous cancellation handling operation.</returns>
    private async Task HandleCancellationFavAsync(List<Deviation> collectedDeviations, string fileName, string username)
    {
        SetProgressStateFav(ProgressState.Idle);

        if (collectedDeviations.Count > 0)
        {
            var result = MessageBox.Show(
                Localizer.GetMessageCancelledPartialFavorites(collectedDeviations.Count),
                Localizer.TitlePartialExport,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    UpdateProgressLabelFav(Localizer.ProgressExporting);
                    await ExportDataFavAsync(collectedDeviations, fileName, username);

                    string exportMessage;
                    if (rbHTMLFav.Checked)
                    {
                        var pageCount = (int)Math.Ceiling(collectedDeviations.Count / 100.0);
                        exportMessage = Localizer.GetMessagePartialExportedHtml(pageCount);
                    }
                    else if (rbXLSXFav.Checked)
                    {
                        exportMessage = Localizer.GetMessagePartialExportedFile($"{fileName}.xlsx");
                    }
                    else
                    {
                        exportMessage = Localizer.GetMessagePartialExportedFile($"{fileName}.csv");
                    }

                    MessageBox.Show(
                        Localizer.GetMessagePartialExported(collectedDeviations.Count, exportMessage),
                        Localizer.TitlePartialComplete,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        Localizer.GetMessagePartialExportError(ex.Message),
                        Localizer.TitleExportError,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
        else
        {
            MessageBox.Show(
                Localizer.MessageCancelledNoFavorites,
                Localizer.TitleCancelled,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// Exports the collected favorited deviations to the selected file format.
    /// </summary>
    /// <param name="deviations">The list of favorited deviations to export.</param>
    /// <param name="fileName">The target filename without extension.</param>
    /// <param name="username">The DeviantArt username for HTML export metadata.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no export format is selected.</exception>
    private async Task ExportDataFavAsync(List<Deviation> deviations, string fileName, string username)
    {
        fileName = RemoveExtensionIfPresent(fileName);

        if (rbCSVFav.Checked)
        {
            await ExportToCsvFavAsync(deviations, fileName);
        }
        else if (rbHTMLFav.Checked)
        {
            var htmlService = new HtmlExportService();
            await htmlService.ExportToHtmlAsync(deviations, fileName, username, isFavorites: true);
        }
        else if (rbXLSXFav.Checked)
        {
            await ExportToXlsxFavAsync(deviations, fileName);
        }
        else
        {
            throw new InvalidOperationException(Localizer.ExceptionNotImplemented);
        }
    }

    /// <summary>
    /// Exports the collected favorited deviations to a CSV file.
    /// </summary>
    /// <param name="deviations">The list of favorited deviations to export.</param>
    /// <param name="fileName">The target filename without extension.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    private async Task ExportToCsvFavAsync(List<Deviation> deviations, string fileName)
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, $"{fileName}.csv");
        var csv = new StringBuilder();

        csv.AppendLine("Title,Author,URL,Published Date,Mature Content,Stats Favourites,Stats Comments,Download URL,Thumbnail URL");

        foreach (var deviation in deviations)
        {
            var publishedDate = deviation.PublishedTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
            var downloadUrl = deviation.Content?.Src ?? "";
            var thumbnailUrl = deviation.Thumbs?.FirstOrDefault()?.Src ?? "";

            csv.AppendLine($"\"{EscapeCsv(deviation.Title ?? "")}\"," +
                           $"\"{EscapeCsv(deviation.Author?.Username ?? "")}\"," +
                           $"\"{deviation.Url ?? ""}\"," +
                           $"\"{publishedDate}\"," +
                           $"{deviation.IsMature}," +
                           $"\"{deviation.Stats?.Favourites ?? 0}\"," +
                           $"\"{deviation.Stats?.Comments ?? 0}\"," +
                           $"\"{downloadUrl}\"," +
                           $"\"{thumbnailUrl}\"");
        }

        await File.WriteAllTextAsync(filePath, csv.ToString(), Encoding.UTF8);
    }

    /// <summary>
    /// Exports the collected favorited deviations to an Excel (XLSX) file.
    /// </summary>
    /// <param name="deviations">The list of favorited deviations to export.</param>
    /// <param name="fileName">The target filename without extension.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    private async Task ExportToXlsxFavAsync(List<Deviation> deviations, string fileName)
    {
        var xlsxService = new XlsxExportService();
        await xlsxService.ExportToXlsxAsync(deviations, fileName);
    }

    /// <summary>
    /// Gets the file filter string for the save dialog based on the selected export format for favorites.
    /// </summary>
    /// <returns>A file filter string for use in file dialogs.</returns>
    private string GetFileFilterFav()
    {
        if (rbCSVFav.Checked)
            return Localizer.FilterCsv;
        if (rbHTMLFav.Checked)
            return Localizer.FilterHtml;
        if (rbXLSXFav.Checked)
            return Localizer.FilterXlsx;

        return Localizer.FilterAll;
    }

    /// <summary>
    /// Gets the default file extension based on the selected export format for favorites.
    /// </summary>
    /// <returns>A file extension string including the leading dot.</returns>
    private string GetFileExtensionFav()
    {
        if (rbCSVFav.Checked)
            return ".csv";
        if (rbHTMLFav.Checked)
            return ".html";
        if (rbXLSXFav.Checked)
            return ".xlsx";

        return "";
    }

    /// <summary>
    /// Sets the visual state of the progress bar and related UI controls for the favorites tab.
    /// </summary>
    /// <param name="state">The desired progress state.</param>
    private void SetProgressStateFav(ProgressState state)
    {
        switch (state)
        {
            case ProgressState.Idle:
                progressScrapingFav.Visible = false;
                lblProgressFav.Visible = false;
                lblTransferRateFav.Visible = false;
                btnCancelScrapingFav.Visible = false;
                btnStartScrapingFav.Enabled = true;
                btnStartScrapingFav.Text = Localizer.ButtonStartScraping;
                break;
            case ProgressState.Running:
                progressScrapingFav.Visible = true;
                progressScrapingFav.Style = ProgressBarStyle.Marquee;
                progressScrapingFav.MarqueeAnimationSpeed = 20;
                lblProgressFav.Visible = true;
                lblProgressFav.Text = Localizer.ProgressInitializing;
                lblTransferRateFav.Visible = false;
                btnCancelScrapingFav.Visible = true;
                btnStartScrapingFav.Enabled = false;
                btnStartScrapingFav.Text = Localizer.ProgressScraping;
                break;
            case ProgressState.Success:
                progressScrapingFav.Style = ProgressBarStyle.Continuous;
                progressScrapingFav.Value = 100;
                lblProgressFav.Text = Localizer.ProgressExportComplete;
                lblTransferRateFav.Visible = false;
                btnStartScrapingFav.Text = Localizer.ProgressComplete;
                btnCancelScrapingFav.Visible = false;
                break;
            case ProgressState.Error:
                progressScrapingFav.Style = ProgressBarStyle.Continuous;
                progressScrapingFav.Value = 0;
                lblProgressFav.Text = Localizer.ProgressError;
                lblTransferRateFav.Visible = false;
                btnStartScrapingFav.Text = Localizer.TitleError;
                btnCancelScrapingFav.Visible = false;
                break;
        }
    }

    /// <summary>
    /// Represents the various states of the scraping progress indicator.
    /// </summary>
    private enum ProgressState
    {
        /// <summary>
        /// No scraping operation is in progress.
        /// </summary>
        Idle,
        
        /// <summary>
        /// A scraping operation is currently running.
        /// </summary>
        Running,
        
        /// <summary>
        /// The scraping operation completed successfully.
        /// </summary>
        Success,
        
        /// <summary>
        /// An error occurred during the scraping operation.
        /// </summary>
        Error
    }
}