using DeviantArtScrapper.Models;

namespace DeviantArtScrapper.Extensions;

/// <summary>
/// Extension methods for Deviation model to work with dates and times.
/// </summary>
public static class DeviationExtensions
{
    /// <summary>
    /// Checks if the deviation was published recently (within specified days).
    /// </summary>
    /// <param name="deviation">The deviation to check.</param>
    /// <param name="days">Number of days to consider as "recent" (default: 7).</param>
    /// <returns>True if published within the specified days, false otherwise.</returns>
    public static bool IsRecent(this Deviation deviation, int days = 7)
    {
        return deviation.PublishedTime.HasValue &&
               (DateTime.UtcNow - deviation.PublishedTime.Value).TotalDays <= days;
    }

    /// <summary>
    /// Gets a human-readable relative time string (e.g., "2 days ago", "3 weeks ago").
    /// </summary>
    /// <param name="deviation">The deviation to get relative time for.</param>
    /// <returns>Relative time string or "Unknown" if no published time.</returns>
    public static string GetRelativeTime(this Deviation deviation)
    {
        if (!deviation.PublishedTime.HasValue)
            return "Unknown";

        var age = DateTime.UtcNow - deviation.PublishedTime.Value;

        if (age.TotalSeconds < 60)
            return "just now";
        if (age.TotalMinutes < 60)
            return $"{(int)age.TotalMinutes} minute{((int)age.TotalMinutes != 1 ? "s" : "")} ago";
        if (age.TotalHours < 24)
            return $"{(int)age.TotalHours} hour{((int)age.TotalHours != 1 ? "s" : "")} ago";
        if (age.TotalDays < 7)
            return $"{(int)age.TotalDays} day{((int)age.TotalDays != 1 ? "s" : "")} ago";
        if (age.TotalDays < 30)
            return $"{(int)(age.TotalDays / 7)} week{((int)(age.TotalDays / 7) != 1 ? "s" : "")} ago";
        if (age.TotalDays < 365)
            return $"{(int)(age.TotalDays / 30)} month{((int)(age.TotalDays / 30) != 1 ? "s" : "")} ago";

        return $"{(int)(age.TotalDays / 365)} year{((int)(age.TotalDays / 365) != 1 ? "s" : "")} ago";
    }

    /// <summary>
    /// Gets the age of the deviation in days.
    /// </summary>
    /// <param name="deviation">The deviation to calculate age for.</param>
    /// <returns>Age in days, or null if no published time.</returns>
    public static double? GetAgeInDays(this Deviation deviation)
    {
        return deviation.PublishedTime.HasValue
            ? (DateTime.UtcNow - deviation.PublishedTime.Value).TotalDays
            : null;
    }

    /// <summary>
    /// Checks if the deviation was published within a specific date range.
    /// </summary>
    /// <param name="deviation">The deviation to check.</param>
    /// <param name="startDate">Start date (inclusive).</param>
    /// <param name="endDate">End date (inclusive).</param>
    /// <returns>True if published within range, false otherwise.</returns>
    public static bool IsPublishedBetween(this Deviation deviation, DateTime startDate, DateTime endDate)
    {
        if (!deviation.PublishedTime.HasValue)
            return false;

        var publishedDate = deviation.PublishedTime.Value;
        return publishedDate >= startDate && publishedDate <= endDate;
    }

    /// <summary>
    /// Formats the published time for display with a specified format.
    /// </summary>
    /// <param name="deviation">The deviation to format time for.</param>
    /// <param name="format">DateTime format string (default: "yyyy-MM-dd HH:mm:ss").</param>
    /// <returns>Formatted date string or "Unknown" if no published time.</returns>
    public static string GetFormattedPublishedTime(this Deviation deviation, string format = "yyyy-MM-dd HH:mm:ss")
    {
        return deviation.PublishedTime?.ToString(format) ?? "Unknown";
    }

    /// <summary>
    /// Gets the published time in local time zone.
    /// </summary>
    /// <param name="deviation">The deviation to get local time for.</param>
    /// <returns>Local DateTime or null if no published time.</returns>
    public static DateTime? GetPublishedTimeLocal(this Deviation deviation)
    {
        return deviation.PublishedTime?.ToLocalTime();
    }

    /// <summary>
    /// Checks if the deviation is a "trending" item (published recently with high engagement).
    /// </summary>
    /// <param name="deviation">The deviation to check.</param>
    /// <param name="daysThreshold">Days to consider as recent (default: 7).</param>
    /// <param name="favouritesThreshold">Minimum favourites to be trending (default: 50).</param>
    /// <returns>True if trending, false otherwise.</returns>
    public static bool IsTrending(this Deviation deviation, int daysThreshold = 7, int favouritesThreshold = 50)
    {
        return deviation.IsRecent(daysThreshold) &&
               deviation.Stats?.Favourites >= favouritesThreshold;
    }
}

/// <summary>
/// Extension methods for filtering collections of Deviations by date.
/// </summary>
public static class DeviationCollectionExtensions
{
    /// <summary>
    /// Filters deviations published within the last specified number of days.
    /// </summary>
    public static IEnumerable<Deviation> PublishedWithinDays(this IEnumerable<Deviation> deviations, int days)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        return deviations.Where(d => d.PublishedTime >= cutoffDate);
    }

    /// <summary>
    /// Filters deviations published within a specific date range.
    /// </summary>
    public static IEnumerable<Deviation> PublishedBetween(this IEnumerable<Deviation> deviations, DateTime startDate, DateTime endDate)
    {
        return deviations.Where(d => d.IsPublishedBetween(startDate, endDate));
    }

    /// <summary>
    /// Orders deviations by published time (newest first).
    /// </summary>
    public static IEnumerable<Deviation> OrderByNewest(this IEnumerable<Deviation> deviations)
    {
        return deviations.OrderByDescending(d => d.PublishedTime ?? DateTime.MinValue);
    }

    /// <summary>
    /// Orders deviations by published time (oldest first).
    /// </summary>
    public static IEnumerable<Deviation> OrderByOldest(this IEnumerable<Deviation> deviations)
    {
        return deviations.OrderBy(d => d.PublishedTime ?? DateTime.MaxValue);
    }

    /// <summary>
    /// Groups deviations by year of publication.
    /// </summary>
    public static IEnumerable<IGrouping<int, Deviation>> GroupByYear(this IEnumerable<Deviation> deviations)
    {
        return deviations
            .Where(d => d.PublishedTime.HasValue)
            .GroupBy(d => d.PublishedTime!.Value.Year);
    }

    /// <summary>
    /// Groups deviations by month of publication.
    /// </summary>
    public static IEnumerable<IGrouping<string, Deviation>> GroupByMonth(this IEnumerable<Deviation> deviations)
    {
        return deviations
            .Where(d => d.PublishedTime.HasValue)
            .GroupBy(d => d.PublishedTime!.Value.ToString("yyyy-MM"));
    }

    /// <summary>
    /// Gets deviations that are trending (recent + high engagement).
    /// </summary>
    public static IEnumerable<Deviation> GetTrending(this IEnumerable<Deviation> deviations, int daysThreshold = 7, int favouritesThreshold = 50)
    {
        return deviations.Where(d => d.IsTrending(daysThreshold, favouritesThreshold));
    }
}
