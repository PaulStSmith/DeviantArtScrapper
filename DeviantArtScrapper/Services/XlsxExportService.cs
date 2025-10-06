using DeviantArtScrapper.Models;
using DeviantArtScrapper.Localization;
using Syncfusion.XlsIO;
using System.Reflection;
using System.Runtime.Versioning;
using Color = Syncfusion.Drawing.Color;

namespace DeviantArtScrapper.Services;

#pragma warning disable CA1822 // Mark members as static

/// <summary>
/// Provides functionality to export DeviantArt deviations to Excel (XLSX) format.
/// </summary>
/// <remarks>
/// Uses Syncfusion.XlsIO library to create professionally formatted Excel workbooks
/// with hyperlinks, date formatting, number formatting, and conditional styling.
/// Requires a valid Syncfusion license embedded as a resource.
/// </remarks>
[SupportedOSPlatform("windows")]
public class XlsxExportService
{
    private static bool _licenseRegistered = false;
    private static readonly Lock _licenseLock = new();

    /// <summary>
    /// Exports deviations to an Excel file with formatting and hyperlinks.
    /// </summary>
    /// <param name="deviations">The list of deviations to export. Must be pre-sorted as desired.</param>
    /// <param name="fileName">The filename for the Excel file (without extension).</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the deviations list is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the Syncfusion license cannot be registered.</exception>
    /// <remarks>
    /// The Excel file includes:
    /// <list type="bullet">
    /// <item>Header row with filtering enabled</item>
    /// <item>Frozen header row for scrolling</item>
    /// <item>Clickable hyperlinks for URLs</item>
    /// <item>Date and number formatting</item>
    /// <item>Conditional formatting for mature content</item>
    /// <item>Auto-fitted columns</item>
    /// </list>
    /// </remarks>
    public async Task ExportToXlsxAsync(List<Deviation> deviations, string fileName)
    {
        if (deviations == null || deviations.Count == 0)
            throw new ArgumentException("No deviations to export", nameof(deviations));

        // Register Syncfusion license
        RegisterSyncfusionLicense();

        // Create Excel engine
        using var excelEngine = new ExcelEngine();
        var application = excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Xlsx;

        // Create workbook and worksheet
        var workbook = application.Workbooks.Create(1);
        var worksheet = workbook.Worksheets[0];
        worksheet.Name = "Gallery";

        // Create header row
        CreateHeaderRow(worksheet);

        // Populate data rows
        await PopulateDataRowsAsync(worksheet, deviations);

        // Apply formatting
        ApplyFormatting(worksheet, deviations.Count);

        // Auto-fit columns
        worksheet.UsedRange.AutofitColumns();

        // Set thumbnail column width (for future image support)
        worksheet.SetColumnWidth(1, 15);

        // Save workbook
        var filePath = Path.Combine(Environment.CurrentDirectory, $"{fileName}.xlsx");
        using var stream = File.Create(filePath);
        workbook.SaveAs(stream);
    }

    /// <summary>
    /// Registers the Syncfusion license from the embedded resource.
    /// </summary>
    /// <remarks>
    /// Thread-safe singleton pattern ensures the license is only registered once.
    /// The license key is loaded from an embedded resource file.
    /// </remarks>
    /// <exception cref="FileNotFoundException">Thrown when the license resource is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when license registration fails.</exception>
    private static void RegisterSyncfusionLicense()
    {
        lock (_licenseLock)
        {
            if (_licenseRegistered)
                return;

            try
            {
                // Load license from embedded resource
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "DeviantArtScrapper.Syncfusion.license";

                using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"Embedded resource '{resourceName}' not found.");
                using var reader = new StreamReader(stream);
                var licenseKey = reader.ReadToEnd().Trim();

                // Register license
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);
                _licenseRegistered = true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to register Syncfusion license", ex);
            }
        }
    }

    /// <summary>
    /// Creates and formats the header row with column names and styling.
    /// </summary>
    /// <param name="worksheet">The worksheet to add the header to.</param>
    private static void CreateHeaderRow(IWorksheet worksheet)
    {
        // Define headers
        var headers = new[]
        {
            Localizer.ExcelColumnTitle,
            Localizer.ExcelColumnAuthor,
            Localizer.ExcelColumnUrl,
            Localizer.ExcelColumnPublishedDate,
            Localizer.ExcelColumnMatureContent,
            Localizer.ExcelColumnFavourites,
            Localizer.ExcelColumnComments,
            Localizer.ExcelColumnDownloadUrl,
            Localizer.ExcelColumnThumbnailUrl
        };

        // Add headers
        for (var i = 0; i < headers.Length; i++)
        {
            worksheet.Range[1, i + 1].Text = headers[i];
        }

        // Format header row
        var headerRange = worksheet.Range[1, 1, 1, headers.Length];
        headerRange.CellStyle.Font.Bold = true;
        headerRange.CellStyle.Font.Size = 11;
        headerRange.CellStyle.Font.Color = ExcelKnownColors.White;
        headerRange.CellStyle.Color = Color.FromArgb(68, 114, 196); // Excel blue
        headerRange.CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
        headerRange.CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;

        // Enable auto-filter
        worksheet.AutoFilters.FilterRange = headerRange;

        // Freeze header row
        worksheet.Range[2, 1].FreezePanes();
    }

    /// <summary>
    /// Populates the worksheet with deviation data including hyperlinks and formatting.
    /// </summary>
    /// <param name="worksheet">The worksheet to populate.</param>
    /// <param name="deviations">The list of deviations to add.</param>
    /// <returns>A completed task.</returns>
    private static Task PopulateDataRowsAsync(IWorksheet worksheet, List<Deviation> deviations)
    {
        var row = 2; // Start after header

        foreach (var deviation in deviations)
        {
            // Title (plain text, no hyperlink)
            worksheet.Range[row, 1].Text = deviation.Title ?? Localizer.ExcelCellUntitled;

            // Author
            worksheet.Range[row, 2].Text = deviation.Author?.Username ?? "";

            // URL (hyperlink with "View" text)
            var urlCell = worksheet.Range[row, 3];
            if (!string.IsNullOrEmpty(deviation.Url))
            {
                urlCell.Text = Localizer.ExcelCellView;
                worksheet.HyperLinks.Add(urlCell, ExcelHyperLinkType.Url, deviation.Url, Localizer.ExcelTooltipView);
                urlCell.CellStyle.Font.Underline = ExcelUnderline.Single;
                urlCell.CellStyle.Font.Color = ExcelKnownColors.Blue;
            }

            // Published Date
            if (deviation.PublishedTime.HasValue)
            {
                worksheet.Range[row, 4].DateTime = deviation.PublishedTime.Value;
                worksheet.Range[row, 4].NumberFormat = "yyyy-mm-dd hh:mm:ss";
            }

            // Mature Content
            worksheet.Range[row, 5].Text = deviation.IsMature ? Localizer.ExcelCellYes : Localizer.ExcelCellNo;
            if (deviation.IsMature)
            {
                worksheet.Range[row, 5].CellStyle.Font.Color = ExcelKnownColors.Red;
                worksheet.Range[row, 5].CellStyle.Font.Bold = true;
            }

            // Favourites (with number formatting)
            worksheet.Range[row, 6].Number = deviation.Stats?.Favourites ?? 0;
            worksheet.Range[row, 6].NumberFormat = "#,##0";

            // Comments (with number formatting)
            worksheet.Range[row, 7].Number = deviation.Stats?.Comments ?? 0;
            worksheet.Range[row, 7].NumberFormat = "#,##0";

            // Download URL (hyperlink with "Download" text)
            var downloadUrl = deviation.Content?.Src ?? "";
            var downloadCell = worksheet.Range[row, 8];
            if (!string.IsNullOrEmpty(downloadUrl))
            {
                downloadCell.Text = Localizer.ExcelCellDownload;
                worksheet.HyperLinks.Add(downloadCell, ExcelHyperLinkType.Url, downloadUrl, Localizer.ExcelTooltipDownload);
                downloadCell.CellStyle.Font.Underline = ExcelUnderline.Single;
                downloadCell.CellStyle.Font.Color = ExcelKnownColors.Blue;
            }

            // Thumbnail URL (hyperlink with "Thumbnail" text)
            var thumbnailUrl = deviation.Thumbs?.FirstOrDefault()?.Src ?? "";
            var thumbnailCell = worksheet.Range[row, 9];
            if (!string.IsNullOrEmpty(thumbnailUrl))
            {
                thumbnailCell.Text = Localizer.ExcelCellThumbnail;
                worksheet.HyperLinks.Add(thumbnailCell, ExcelHyperLinkType.Url, thumbnailUrl, Localizer.ExcelTooltipThumbnail);
                thumbnailCell.CellStyle.Font.Underline = ExcelUnderline.Single;
                thumbnailCell.CellStyle.Font.Color = ExcelKnownColors.Blue;
            }

            // Alternate row coloring for mature content
            if (deviation.IsMature)
            {
                var rowRange = worksheet.Range[row, 1, row, 9];
                rowRange.CellStyle.ColorIndex = ExcelKnownColors.Light_orange;
            }

            row++;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Applies formatting, borders, and alignment to the worksheet.
    /// </summary>
    /// <param name="worksheet">The worksheet to format.</param>
    /// <param name="dataRowCount">The number of data rows (excluding header).</param>
    private static void ApplyFormatting(IWorksheet worksheet, int dataRowCount)
    {
        // Apply borders to all used cells
        var usedRange = worksheet.Range[1, 1, dataRowCount + 1, 9];
        usedRange.BorderAround(ExcelLineStyle.Thin, ExcelKnownColors.Grey_25_percent);
        usedRange.BorderInside(ExcelLineStyle.Thin, ExcelKnownColors.Grey_25_percent);

        // Center-align certain columns
        worksheet.Range[2, 5, dataRowCount + 1, 5].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter; // Mature
        worksheet.Range[2, 6, dataRowCount + 1, 6].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;  // Favourites
        worksheet.Range[2, 7, dataRowCount + 1, 7].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignRight;  // Comments

        // Wrap text in URL columns
        worksheet.Range[2, 3, dataRowCount + 1, 3].WrapText = false; // URL
        worksheet.Range[2, 8, dataRowCount + 1, 8].WrapText = false; // Download URL
        worksheet.Range[2, 9, dataRowCount + 1, 9].WrapText = false; // Thumbnail URL

        // Set row height for header
        worksheet.SetRowHeight(1, 25);
    }
}
