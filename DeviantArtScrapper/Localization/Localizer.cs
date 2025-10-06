using System.Globalization;
using DeviantArtScrapper.Properties;

namespace DeviantArtScrapper.Localization;

/// <summary>
/// Provides centralized access to localized strings for the application.
/// Automatically loads resources based on the current UI culture.
/// </summary>
public static class Localizer
{
    #region Main Form Strings

    /// <summary>
    /// Gets the main form title: "DeviantArt Scrapper"
    /// </summary>
    public static string MainFormTitle => Resources.MainForm_Title;

    /// <summary>
    /// Gets the status message when API is ready and authenticated
    /// </summary>
    public static string StatusReady => Resources.MainForm_StatusReady;

    /// <summary>
    /// Gets the status message when API is configured but not authenticated
    /// </summary>
    public static string StatusConfigured => Resources.MainForm_StatusConfigured;

    /// <summary>
    /// Gets the status message when API is not configured
    /// </summary>
    public static string StatusNotConfigured => Resources.MainForm_StatusNotConfigured;

    #endregion

    #region Tab Names

    /// <summary>
    /// Gets the Gallery tab name
    /// </summary>
    public static string TabGallery => Resources.Tab_Gallery;

    /// <summary>
    /// Gets the Favorites tab name
    /// </summary>
    public static string TabFavorites => Resources.Tab_Favorites;

    #endregion

    #region Common Labels

    /// <summary>
    /// Gets the username label text
    /// </summary>
    public static string LabelUsername => Resources.Label_Username;

    /// <summary>
    /// Gets the filename label text
    /// </summary>
    public static string LabelFileName => Resources.Label_FileName;

    /// <summary>
    /// Gets the file format label text
    /// </summary>
    public static string LabelFileFormat => Resources.Label_FileFormat;

    #endregion

    #region Button Text

    /// <summary>
    /// Gets the Browse button text
    /// </summary>
    public static string ButtonBrowse => Resources.Button_Browse;

    /// <summary>
    /// Gets the Start Scraping button text
    /// </summary>
    public static string ButtonStartScraping => Resources.Button_StartScraping;

    /// <summary>
    /// Gets the Cancel button text
    /// </summary>
    public static string ButtonCancel => Resources.Button_Cancel;

    /// <summary>
    /// Gets the Settings button text
    /// </summary>
    public static string ButtonSettings => Resources.Button_Settings;

    /// <summary>
    /// Gets the Save button text
    /// </summary>
    public static string ButtonSave => Resources.Button_Save;

    /// <summary>
    /// Gets the Test Connection button text
    /// </summary>
    public static string ButtonTestConnection => Resources.Button_TestConnection;

    #endregion

    #region Progress Messages

    /// <summary>
    /// Gets the initializing progress message
    /// </summary>
    public static string ProgressInitializing => Resources.Progress_Initializing;

    /// <summary>
    /// Gets the collected items progress message with count
    /// </summary>
    public static string GetProgressCollected(int count) => 
        string.Format(Resources.Progress_Collected, count);

    /// <summary>
    /// Gets the collected favorites progress message with count
    /// </summary>
    public static string GetProgressCollectedFavorites(int count) => 
        string.Format(Resources.Progress_CollectedFavorites, count);

    /// <summary>
    /// Gets the exporting data progress message
    /// </summary>
    public static string ProgressExporting => Resources.Progress_Exporting;

    /// <summary>
    /// Gets the cancelling progress message
    /// </summary>
    public static string ProgressCancelling => Resources.Progress_Cancelling;

    /// <summary>
    /// Gets the export complete progress message
    /// </summary>
    public static string ProgressExportComplete => Resources.Progress_ExportComplete;

    /// <summary>
    /// Gets the error occurred progress message
    /// </summary>
    public static string ProgressError => Resources.Progress_Error;

    /// <summary>
    /// Gets the scraping in progress message
    /// </summary>
    public static string ProgressScraping => Resources.Progress_Scraping;

    /// <summary>
    /// Gets the complete progress message
    /// </summary>
    public static string ProgressComplete => Resources.Progress_Complete;

    /// <summary>
    /// Gets the request failed progress message with retry information
    /// </summary>
    public static string GetProgressRequestFailed(int attempt, int maxAttempts, int delayMs) =>
        string.Format(Resources.Progress_RequestFailed, attempt, maxAttempts, delayMs);

    #endregion

    #region MessageBox Titles

    /// <summary>
    /// Gets the Information dialog title
    /// </summary>
    public static string TitleInformation => Resources.MessageBox_Information;

    /// <summary>
    /// Gets the Validation Error dialog title
    /// </summary>
    public static string TitleValidationError => Resources.MessageBox_ValidationError;

    /// <summary>
    /// Gets the Configuration Required dialog title
    /// </summary>
    public static string TitleConfigRequired => Resources.MessageBox_ConfigRequired;

    /// <summary>
    /// Gets the No Results dialog title
    /// </summary>
    public static string TitleNoResults => Resources.MessageBox_NoResults;

    /// <summary>
    /// Gets the Export Complete dialog title
    /// </summary>
    public static string TitleExportComplete => Resources.MessageBox_ExportComplete;

    /// <summary>
    /// Gets the Error dialog title
    /// </summary>
    public static string TitleError => Resources.MessageBox_Error;

    /// <summary>
    /// Gets the Success dialog title
    /// </summary>
    public static string TitleSuccess => Resources.MessageBox_Success;

    /// <summary>
    /// Gets the Cancelled dialog title
    /// </summary>
    public static string TitleCancelled => Resources.MessageBox_Cancelled;

    /// <summary>
    /// Gets the Partial Export dialog title
    /// </summary>
    public static string TitlePartialExport => Resources.MessageBox_PartialExport;

    /// <summary>
    /// Gets the Partial Export Complete dialog title
    /// </summary>
    public static string TitlePartialComplete => Resources.MessageBox_PartialComplete;

    /// <summary>
    /// Gets the Export Error dialog title
    /// </summary>
    public static string TitleExportError => Resources.MessageBox_ExportError;

    #endregion

    #region MessageBox Messages

    /// <summary>
    /// Gets the message indicating scraping is already in progress
    /// </summary>
    public static string MessageScrapingInProgress => Resources.Message_ScrapingInProgress;

    /// <summary>
    /// Gets the message prompting to enter a username
    /// </summary>
    public static string MessageEnterUsername => Resources.Message_EnterUsername;

    /// <summary>
    /// Gets the message prompting to enter a filename
    /// </summary>
    public static string MessageEnterFilename => Resources.Message_EnterFilename;

    /// <summary>
    /// Gets the message prompting to configure API credentials
    /// </summary>
    public static string MessageConfigureApi => Resources.Message_ConfigureApi;

    /// <summary>
    /// Gets the message indicating no gallery items were found
    /// </summary>
    public static string GetMessageNoGalleryItems(string username) =>
        string.Format(Resources.Message_NoGalleryItems, username);

    /// <summary>
    /// Gets the message indicating no favorites were found
    /// </summary>
    public static string GetMessageNoFavorites(string username) =>
        string.Format(Resources.Message_NoFavorites, username);

    /// <summary>
    /// Gets the scraping error message
    /// </summary>
    public static string GetMessageScrapingError(string error) =>
        string.Format(Resources.Message_ScrapingError, error);

    /// <summary>
    /// Gets the success message for scraped items
    /// </summary>
    public static string GetMessageSuccessScraped(int count, string filename) =>
        string.Format(Resources.Message_SuccessScraped, count, filename);

    /// <summary>
    /// Gets the success message for scraped items to HTML
    /// </summary>
    public static string GetMessageSuccessScrapedHtml(int count, int pageCount) =>
        string.Format(Resources.Message_SuccessScrapedHtml, count, pageCount);

    /// <summary>
    /// Gets the success message for scraped favorites to HTML
    /// </summary>
    public static string GetMessageSuccessScrapedFavoritesHtml(int count, int pageCount) =>
        string.Format(Resources.Message_SuccessScrapedFavoritesHtml, count, pageCount);

    /// <summary>
    /// Gets the success message for scraped favorites
    /// </summary>
    public static string GetMessageSuccessScrapedFavorites(int count, string filename) =>
        string.Format(Resources.Message_SuccessScrapedFavorites, count, filename);

    /// <summary>
    /// Gets the message for cancelled scraping with partial results
    /// </summary>
    public static string GetMessageCancelledPartial(int count) =>
        string.Format(Resources.Message_CancelledPartial, count);

    /// <summary>
    /// Gets the message for cancelled favorites scraping with partial results
    /// </summary>
    public static string GetMessageCancelledPartialFavorites(int count) =>
        string.Format(Resources.Message_CancelledPartialFavorites, count);

    /// <summary>
    /// Gets the message for cancelled scraping with no items collected
    /// </summary>
    public static string MessageCancelledNoItems => Resources.Message_CancelledNoItems;

    /// <summary>
    /// Gets the message for cancelled favorites scraping with no items collected
    /// </summary>
    public static string MessageCancelledNoFavorites => Resources.Message_CancelledNoFavorites;

    /// <summary>
    /// Gets the message for successfully exported partial results
    /// </summary>
    public static string GetMessagePartialExported(int count, string details) =>
        string.Format(Resources.Message_PartialExported, count, details);

    /// <summary>
    /// Gets the message for partial HTML export
    /// </summary>
    public static string GetMessagePartialExportedHtml(int pageCount) =>
        string.Format(Resources.Message_PartialExportedHtml, pageCount);

    /// <summary>
    /// Gets the message for partial file export
    /// </summary>
    public static string GetMessagePartialExportedFile(string filename) =>
        string.Format(Resources.Message_PartialExportedFile, filename);

    /// <summary>
    /// Gets the error message for partial export failure
    /// </summary>
    public static string GetMessagePartialExportError(string error) =>
        string.Format(Resources.Message_PartialExportError, error);

    #endregion

    #region Settings Form Strings

    /// <summary>
    /// Gets the settings form title
    /// </summary>
    public static string SettingsTitle => Resources.Settings_Title;

    /// <summary>
    /// Gets the settings window title
    /// </summary>
    public static string SettingsWindowTitle => Resources.Settings_WindowTitle;

    /// <summary>
    /// Gets the API settings group title
    /// </summary>
    public static string SettingsApiSettings => Resources.Settings_ApiSettings;

    /// <summary>
    /// Gets the Client ID label
    /// </summary>
    public static string SettingsClientId => Resources.Settings_ClientId;

    /// <summary>
    /// Gets the Client Secret label
    /// </summary>
    public static string SettingsClientSecret => Resources.Settings_ClientSecret;

    /// <summary>
    /// Gets the Authentication Status group title
    /// </summary>
    public static string SettingsAuthStatus => Resources.Settings_AuthStatus;

    /// <summary>
    /// Gets the Token Status label
    /// </summary>
    public static string SettingsTokenStatus => Resources.Settings_TokenStatus;

    /// <summary>
    /// Gets the no token available message
    /// </summary>
    public static string SettingsNoToken => Resources.Settings_NoToken;

    /// <summary>
    /// Gets the token valid until message
    /// </summary>
    public static string GetSettingsTokenValid(DateTime expiration) =>
        string.Format(Resources.Settings_TokenValid, expiration.ToString("g"));

    /// <summary>
    /// Gets the connection successful message
    /// </summary>
    public static string SettingsConnectionSuccess => Resources.Settings_ConnectionSuccess;

    /// <summary>
    /// Gets the connection failed message
    /// </summary>
    public static string GetSettingsConnectionFailed(string error) =>
        string.Format(Resources.Settings_ConnectionFailed, error);

    /// <summary>
    /// Gets the settings saved successfully message
    /// </summary>
    public static string SettingsSavedSuccess => Resources.Settings_SavedSuccess;

    /// <summary>
    /// Gets the settings save failed message
    /// </summary>
    public static string GetSettingsSaveFailed(string error) =>
        string.Format(Resources.Settings_SaveFailed, error);

    /// <summary>
    /// Gets the configuration load failed message
    /// </summary>
    public static string GetSettingsLoadFailed(string error) =>
        string.Format(Resources.Settings_LoadFailed, error);

    /// <summary>
    /// Gets the enter credentials prompt message
    /// </summary>
    public static string SettingsEnterCredentials => Resources.Settings_EnterCredentials;

    #endregion

    #region File Filters

    /// <summary>
    /// Gets the CSV file filter for save dialogs
    /// </summary>
    public static string FilterCsv => Resources.Filter_Csv;

    /// <summary>
    /// Gets the HTML file filter for save dialogs
    /// </summary>
    public static string FilterHtml => Resources.Filter_Html;

    /// <summary>
    /// Gets the Excel file filter for save dialogs
    /// </summary>
    public static string FilterXlsx => Resources.Filter_Xlsx;

    /// <summary>
    /// Gets the all files filter for save dialogs
    /// </summary>
    public static string FilterAll => Resources.Filter_All;

    #endregion

    #region Utility Methods

    /// <summary>
    /// Gets the current UI culture name
    /// </summary>
    public static string CurrentCulture => CultureInfo.CurrentUICulture.Name;

    /// <summary>
    /// Checks if the current culture is Portuguese (Brazilian)
    /// </summary>
    public static bool IsPortuguese => 
        CultureInfo.CurrentUICulture.Name.StartsWith("pt", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Sets the UI culture for the application
    /// </summary>
    /// <param name="cultureName">The culture name (e.g., "en", "pt-BR")</param>
    public static void SetCulture(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        CultureInfo.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    #endregion
}
