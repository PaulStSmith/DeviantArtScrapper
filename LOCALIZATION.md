# Localization Guide for DeviantArt Scrapper

This guide explains how to use the resource files (RESX) for localizing the DeviantArt Scrapper application.

## Overview

The application now includes localization support through .NET resource files (RESX). Two languages are currently supported:

- **English (en)** - Default language (`Resources.resx`)
- **Portuguese (Brazilian) (pt-BR)** - Portuguese localization (`Resources.pt-BR.resx`)

## Resource Files Location

Resource files are located in:
```
DeviantArtScrapper/Properties/
??? Resources.resx (English - Default)
??? Resources.pt-BR.resx (Portuguese Brazilian)
```

## How to Use Resource Strings in Code

### 1. Add Using Statement

```csharp
using DeviantArtScrapper.Properties;
```

### 2. Access Resource Strings

Instead of hardcoded strings:
```csharp
// Old way (hardcoded)
MessageBox.Show("Scraping is already in progress.", "Information",
    MessageBoxButtons.OK, MessageBoxIcon.Information);
```

Use resource strings:
```csharp
// New way (localized)
MessageBox.Show(Resources.Message_ScrapingInProgress, Resources.MessageBox_Information,
    MessageBoxButtons.OK, MessageBoxIcon.Information);
```

### 3. Format Strings with Parameters

For messages with dynamic content:
```csharp
// Resource string: "Collected {0} items..."
UpdateProgressLabel(string.Format(Resources.Progress_Collected, allDeviations.Count));
```

Or using string interpolation (C# 10+):
```csharp
MessageBox.Show(
    string.Format(Resources.Message_NoGalleryItems, username),
    Resources.MessageBox_NoResults,
    MessageBoxButtons.OK,
    MessageBoxIcon.Information);
```

## Available Resource Categories

### Main Form Strings
- `MainForm_Title` - Application title
- `MainForm_StatusReady` - Ready status message
- `MainForm_StatusConfigured` - Configured status message
- `MainForm_StatusNotConfigured` - Not configured status message

### Tab Names
- `Tab_Gallery` - Gallery tab name
- `Tab_Favorites` - Favorites tab name

### Common Labels
- `Label_Username` - Username label
- `Label_FileName` - File name label
- `Label_FileFormat` - File format label

### Button Text
- `Button_Browse` - Browse button
- `Button_StartScraping` - Start scraping button
- `Button_Cancel` - Cancel button
- `Button_Settings` - Settings button
- `Button_Save` - Save button
- `Button_TestConnection` - Test connection button

### Progress Messages
- `Progress_Initializing` - "Initializing..."
- `Progress_Collected` - "Collected {0} items..."
- `Progress_CollectedFavorites` - "Collected {0} favorites..."
- `Progress_Exporting` - "Exporting data..."
- `Progress_Cancelling` - "Cancelling..."
- `Progress_ExportComplete` - "Export complete!"
- `Progress_Error` - "Error occurred"

### MessageBox Titles
- `MessageBox_Information` - Information dialog title
- `MessageBox_ValidationError` - Validation error title
- `MessageBox_ConfigRequired` - Configuration required title
- `MessageBox_NoResults` - No results title
- `MessageBox_ExportComplete` - Export complete title
- `MessageBox_Error` - Error dialog title
- `MessageBox_Success` - Success dialog title

### MessageBox Messages
- `Message_ScrapingInProgress` - Scraping already in progress message
- `Message_EnterUsername` - Enter username prompt
- `Message_EnterFilename` - Enter filename prompt
- `Message_ConfigureApi` - Configure API credentials message
- `Message_NoGalleryItems` - No gallery items found message
- `Message_NoFavorites` - No favorites found message
- `Message_ScrapingError` - Scraping error message
- `Message_SuccessScraped` - Success message for scraping
- `Message_CancelledPartial` - Cancellation with partial results message

### Settings Form Strings
- `Settings_Title` - Settings form title
- `Settings_WindowTitle` - Settings window title
- `Settings_ApiSettings` - API settings group title
- `Settings_ClientId` - Client ID label
- `Settings_ClientSecret` - Client Secret label
- `Settings_AuthStatus` - Authentication status group title
- `Settings_TokenStatus` - Token status label
- `Settings_ConnectionSuccess` - Connection successful message
- `Settings_ConnectionFailed` - Connection failed message
- `Settings_SavedSuccess` - Settings saved message
- `Settings_SaveFailed` - Settings save failed message

### File Filters
- `Filter_Csv` - CSV file filter
- `Filter_Html` - HTML file filter
- `Filter_Xlsx` - Excel file filter
- `Filter_All` - All files filter

## How the Application Determines Language

The application automatically detects the system language using .NET's `CultureInfo.CurrentUICulture`. 

- If the system is set to Portuguese (Brazil), it loads `Resources.pt-BR.resx`
- For all other languages, it falls back to `Resources.resx` (English)

## Adding a New Language

To add support for another language (e.g., Spanish):

1. **Create a new RESX file**:
   ```
   DeviantArtScrapper/Properties/Resources.es.resx
   ```

2. **Copy the structure** from `Resources.resx`

3. **Translate all `<value>` elements** to Spanish

4. **.NET will automatically** load the correct file based on system language

## Testing Different Languages

### In Windows

1. **Change Windows Display Language**:
   - Settings ? Time & Language ? Language
   - Add Portuguese (Brazil) or your target language
   - Set as display language
   - Restart the application

2. **Or use Regional Settings**:
   - Control Panel ? Region ? Administrative
   - Change system locale

### In Code (For Testing)

Add this to `Program.cs` before creating the main form:

```csharp
// Force Portuguese for testing
System.Threading.Thread.CurrentThread.CurrentUICulture = 
    new System.Globalization.CultureInfo("pt-BR");

// Force English for testing
System.Threading.Thread.CurrentThread.CurrentUICulture = 
    new System.Globalization.CultureInfo("en");
```

## Implementation Checklist

To fully implement localization in your application:

- [ ] Update `MainForm.cs` to use `Resources.*` instead of hardcoded strings
- [ ] Update `MainForm.Designer.cs` to use `Resources.*` for control text
- [ ] Update `SettingsForm.cs` to use `Resources.*` instead of hardcoded strings
- [ ] Update `SettingsForm.Designer.cs` to use `Resources.*` for control text
- [ ] Update all `MessageBox.Show()` calls throughout the application
- [ ] Update all progress messages and labels
- [ ] Test with both English and Portuguese Windows settings
- [ ] Add XML comments referencing resource keys for maintainability

## Best Practices

1. **Never hardcode user-facing strings** - Always use resource files
2. **Use descriptive resource keys** - e.g., `Message_NoGalleryItems` instead of `Msg1`
3. **Group related strings** - Use prefixes like `MessageBox_`, `Button_`, `Label_`
4. **Keep formatting consistent** - Use `{0}`, `{1}` for string.Format parameters
5. **Test both languages** - Ensure UI doesn't break with longer translations
6. **Update documentation** - Keep this guide updated when adding new strings

## Example: Converting Hardcoded Strings

### Before (Hardcoded):
```csharp
private void UpdateStatusLabel()
{
    if (hasCredentials && hasToken)
        lblStatus.Text = "Status: Ready - API configured and authenticated";
    else if (hasCredentials)
        lblStatus.Text = "Status: Configured - Authentication required";
    else
        lblStatus.Text = "Status: Not configured - Please configure API settings";
}
```

### After (Localized):
```csharp
private void UpdateStatusLabel()
{
    if (hasCredentials && hasToken)
        lblStatus.Text = Resources.MainForm_StatusReady;
    else if (hasCredentials)
        lblStatus.Text = Resources.MainForm_StatusConfigured;
    else
        lblStatus.Text = Resources.MainForm_StatusNotConfigured;
}
```

## NSIS Installer Localization

The NSIS installer (`DeviantArtScrapper.nsi`) already includes:

- ? Portuguese (Brazilian) language support
- ? Automatic language detection
- ? Language-specific license files (LICENSE and LICENSE.pt-BR)
- ? Language-specific manual files (USER_MANUAL.md and MANUAL_USUARIO.md)
- ? Localized installer messages and prompts

The installer will automatically:
- Show in Portuguese on Brazilian Portuguese Windows systems
- Install `MANUAL_USUARIO.md` for Portuguese users
- Install `USER_MANUAL.md` for English/other users
- Display the appropriate LICENSE file during installation
- Create Start Menu shortcuts with correct language

## License Files

Two license files are included:

- `LICENSE` - MIT License in English
- `LICENSE.pt-BR` - MIT License in Portuguese (Brazilian)

The installer automatically selects and installs the appropriate license based on the system language.

## Questions or Issues?

If you encounter any issues with localization:

1. Check that resource files are marked as "Embedded Resource" in project properties
2. Verify the resource file namespace matches your project
3. Ensure you're using `Resources.ResourceName` (not `ResourceManager` directly)
4. Build the project to regenerate the `Resources.Designer.cs` file

---

**Note**: After modifying RESX files in Visual Studio, the `Resources.Designer.cs` file is automatically regenerated. You should never manually edit the `.Designer.cs` file.
