using DeviantArtScrapper.Extensions;
using DeviantArtScrapper.Models;
using System.Runtime.Versioning;
using System.Text;

namespace DeviantArtScrapper.Services;

#pragma warning disable CA1822 // Mark members as static

/// <summary>
/// Provides functionality to export DeviantArt deviations to paginated HTML gallery files.
/// </summary>
/// <remarks>
/// This service generates professional-looking HTML galleries with dark theme styling,
/// pagination support, and responsive design. Templates are loaded from embedded resources.
/// </remarks>
[SupportedOSPlatform("windows")]
public class HtmlExportService
{
    private const int ItemsPerPage = 100;
    private static readonly string[] ColorPalette =
    [
        "#1a1a1a", "#202020", "#252525", "#1c1c1c", "#222222",
        "#1e1e1e", "#242424", "#1b1b1b", "#212121", "#1d1d1d"
    ];

    /// <summary>
    /// Exports deviations to HTML gallery files with pagination.
    /// </summary>
    /// <param name="deviations">The list of deviations to export. Must be pre-sorted as desired.</param>
    /// <param name="baseFileName">The base filename for the HTML files (without extension).</param>
    /// <param name="username">The DeviantArt username for display in the gallery header.</param>
    /// <param name="isFavorites">If true, uses the favorites template and displays author names; otherwise uses the gallery template.</param>
    /// <returns>A task representing the asynchronous export operation.</returns>
    /// <exception cref="ArgumentException">Thrown when the deviations list is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the HTML template resource cannot be found.</exception>
    /// <remarks>
    /// Generates multiple HTML files if there are more than 100 deviations.
    /// First page is named "{baseFileName}.html", subsequent pages are named "{baseFileName}_page_N.html".
    /// </remarks>
    public async Task ExportToHtmlAsync(
        List<Deviation> deviations,
        string baseFileName,
        string username,
        bool isFavorites = false)
    {
        if (deviations == null || deviations.Count == 0)
            throw new ArgumentException("No deviations to export", nameof(deviations));

        // Load template
        var template = await LoadTemplateAsync(isFavorites);

        // Calculate pagination
        var totalPages = (int)Math.Ceiling(deviations.Count / (double)ItemsPerPage);
        var scrapeDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Generate each page
        for (var pageNumber = 1; pageNumber <= totalPages; pageNumber++)
        {
            var pageDeviations = deviations
                .Skip((pageNumber - 1) * ItemsPerPage)
                .Take(ItemsPerPage)
                .ToList();

            var html = await GeneratePageHtmlAsync(
                template,
                pageDeviations,
                username,
                scrapeDate,
                deviations.Count,
                pageNumber,
                totalPages,
                baseFileName,
                isFavorites);

            var fileName = pageNumber == 1
                ? $"{baseFileName}.html"
                : $"{baseFileName}_page_{pageNumber}.html";

            var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
            await File.WriteAllTextAsync(filePath, html, Encoding.UTF8);
        }
    }

    /// <summary>
    /// Loads the HTML template from embedded resources.
    /// </summary>
    /// <param name="isFavorites">If true, loads the favorites template; otherwise loads the gallery template.</param>
    /// <returns>A task containing the template HTML as a string.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the requested template resource is not found in the assembly.</exception>
    private static async Task<string> LoadTemplateAsync(bool isFavorites = false)
    {
        var templateName = isFavorites ? "favorites_template.html" : "gallery_template.html";
        var assembly = typeof(HtmlExportService).Assembly;
        var resourceName = $"DeviantArtScrapper.Resources.{templateName}";

        await using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"Embedded resource not found: {resourceName}");
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Generates the complete HTML for a single page of deviations.
    /// </summary>
    /// <param name="template">The base HTML template.</param>
    /// <param name="pageDeviations">The deviations to include on this page.</param>
    /// <param name="username">The username for display.</param>
    /// <param name="scrapeDate">The date/time when scraping occurred.</param>
    /// <param name="totalDeviations">The total number of deviations across all pages.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="totalPages">The total number of pages.</param>
    /// <param name="baseFileName">The base filename for page links.</param>
    /// <param name="isFavorites">Whether this is a favorites gallery (affects author display).</param>
    /// <returns>A task containing the complete HTML with all placeholders replaced.</returns>
    private async Task<string> GeneratePageHtmlAsync(
        string template,
        List<Deviation> pageDeviations,
        string username,
        string scrapeDate,
        int totalDeviations,
        int pageNumber,
        int totalPages,
        string baseFileName,
        bool isFavorites)
    {
        var html = template;

        // Replace global placeholders
        html = html.Replace("{{USERNAME}}", EscapeHtml(username));
        html = html.Replace("{{SCRAPE_DATE}}", scrapeDate);
        html = html.Replace("{{TOTAL_DEVIATIONS}}", totalDeviations.ToString("N0"));
        html = html.Replace("{{PAGE_NUMBER}}", pageNumber.ToString());
        html = html.Replace("{{TOTAL_PAGES}}", totalPages.ToString());
        html = html.Replace("{{ITEMS_ON_PAGE}}", pageDeviations.Count.ToString());

        // Generate pagination links
        var prevPage = pageNumber > 1
            ? (pageNumber == 2 ? $"{baseFileName}.html" : $"{baseFileName}_page_{pageNumber - 1}.html")
            : "#";
        var nextPage = pageNumber < totalPages
            ? $"{baseFileName}_page_{pageNumber + 1}.html"
            : "#";

        html = html.Replace("{{PREV_PAGE_FILE}}", prevPage);
        html = html.Replace("{{NEXT_PAGE_FILE}}", nextPage);
        html = html.Replace("{{PREV_DISABLED}}", pageNumber == 1 ? "disabled" : "");
        html = html.Replace("{{NEXT_DISABLED}}", pageNumber == totalPages ? "disabled" : "");

        // Generate page number links
        var pageLinks = GeneratePageLinks(pageNumber, totalPages, baseFileName);
        html = html.Replace("{{PAGE_LINKS}}", pageLinks);

        // Generate deviation cards
        var deviationItems = await GenerateDeviationCardsAsync(pageDeviations, isFavorites);
        html = html.Replace("{{DEVIATION_ITEMS}}", deviationItems);

        return html;
    }

    /// <summary>
    /// Generates pagination links for navigating between pages.
    /// </summary>
    /// <param name="currentPage">The current page number.</param>
    /// <param name="totalPages">The total number of pages.</param>
    /// <param name="baseFileName">The base filename for generating page links.</param>
    /// <returns>HTML string containing pagination links.</returns>
    /// <remarks>
    /// Shows up to 7 page links. If there are more pages, shows first 5, ellipsis, and last page.
    /// </remarks>
    private static string GeneratePageLinks(int currentPage, int totalPages, string baseFileName)
    {
        var links = new StringBuilder();
        var maxLinks = 7; // Show max 7 page links

        if (totalPages <= maxLinks)
        {
            // Show all pages
            for (var i = 1; i <= totalPages; i++)
            {
                links.Append(GeneratePageLink(i, currentPage, baseFileName));
            }
        }
        else
        {
            // Show first 5, ellipsis, last
            var showPages = 5;
            for (var i = 1; i <= Math.Min(showPages, totalPages); i++)
            {
                links.Append(GeneratePageLink(i, currentPage, baseFileName));
            }

            if (totalPages > showPages)
            {
                links.Append("<span class=\"pagination-info\">...</span>");
                links.Append(GeneratePageLink(totalPages, currentPage, baseFileName));
            }
        }

        return links.ToString();
    }

    /// <summary>
    /// Generates a single pagination link.
    /// </summary>
    /// <param name="pageNumber">The page number for this link.</param>
    /// <param name="currentPage">The current page number (for active state).</param>
    /// <param name="baseFileName">The base filename for the link href.</param>
    /// <returns>HTML anchor tag for the page link.</returns>
    private static string GeneratePageLink(int pageNumber, int currentPage, string baseFileName)
    {
        var fileName = pageNumber == 1
            ? $"{baseFileName}.html"
            : $"{baseFileName}_page_{pageNumber}.html";

        var activeClass = pageNumber == currentPage ? " active" : "";

        return $"<a href=\"{fileName}\" class=\"pagination-btn{activeClass}\">{pageNumber}</a>";
    }

    /// <summary>
    /// Generates HTML cards for displaying deviations.
    /// </summary>
    /// <param name="deviations">The list of deviations to generate cards for.</param>
    /// <param name="isFavorites">If true, includes author names in the cards.</param>
    /// <returns>A task containing the HTML string with all deviation cards.</returns>
    private async Task<string> GenerateDeviationCardsAsync(List<Deviation> deviations, bool isFavorites)
    {
        var cards = new StringBuilder();
        var colorIndex = 0;

        foreach (var deviation in deviations)
        {
            var thumbnailUrl = deviation.Thumbs?.FirstOrDefault()?.Src
                            ?? deviation.Content?.Src
                            ?? "";

            var publishedDate = deviation.PublishedTime?.ToString("MMM dd, yyyy") ?? "Unknown";
            var title = EscapeHtml(deviation.Title ?? "Untitled");
            var author = EscapeHtml(deviation.Author?.Username ?? "Unknown");
            var url = deviation.Url ?? "";
            var favourites = deviation.Stats?.Favourites ?? 0;
            var comments = deviation.Stats?.Comments ?? 0;
            var isMature = deviation.IsMature;

            // Select color for placeholder
            var color = ColorPalette[colorIndex % ColorPalette.Length];
            colorIndex++;

            // Build card HTML
            cards.AppendLine($"<div class=\"deviation-card\" data-url=\"{EscapeHtml(url)}\">");
            cards.AppendLine("    <div class=\"deviation-thumbnail\" style=\"background: linear-gradient(135deg, " + color + " 0%, " + color + "dd 100%);\">");

            if (isMature)
            {
                cards.AppendLine("        <div class=\"mature-badge\">18+</div>");
            }

            if (!string.IsNullOrEmpty(thumbnailUrl))
            {
                cards.AppendLine($"        <img src=\"{EscapeHtml(thumbnailUrl)}\" alt=\"{title}\" onerror=\"this.style.display='none';this.nextElementSibling.style.display='block';\" />");
                cards.AppendLine($"        <span style=\"opacity: 0.7; display: none;\">{title}</span>");
            }
            else
            {
                cards.AppendLine($"        <span style=\"opacity: 0.7;\">{title}</span>");
            }

            cards.AppendLine("    </div>");
            cards.AppendLine("    <div class=\"deviation-info\">");
            cards.AppendLine($"        <div class=\"deviation-title\" title=\"{title}\">{title}</div>");
            
            // Add author byline for favorites
            if (isFavorites)
            {
                cards.AppendLine($"        <div class=\"deviation-author\">{author}</div>");
            }
            
            cards.AppendLine("        <div class=\"deviation-meta\">");
            cards.AppendLine($"            <span>{publishedDate}</span>");
            cards.AppendLine("        </div>");
            cards.AppendLine("        <div class=\"deviation-stats\">");
            cards.AppendLine("            <div class=\"stat-item\">");
            cards.AppendLine("                <span class=\"icon\">&#10084;&#65039;</span>");
            cards.AppendLine($"                <span>{favourites}</span>");
            cards.AppendLine("            </div>");
            cards.AppendLine("            <div class=\"stat-item\">");
            cards.AppendLine("                <span class=\"icon\">&#128172;</span>");
            cards.AppendLine($"                <span>{comments}</span>");
            cards.AppendLine("            </div>");
            cards.AppendLine("        </div>");
            cards.AppendLine("    </div>");
            cards.AppendLine("</div>");
            cards.AppendLine();
        }

        await Task.CompletedTask; // Just to keep the method async

        return cards.ToString();
    }

    /// <summary>
    /// Escapes special HTML characters to prevent XSS and display issues.
    /// </summary>
    /// <param name="text">The text to escape.</param>
    /// <returns>The HTML-safe escaped text.</returns>
    private static string EscapeHtml(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }
}
