using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeviantArtScrapper.Converters;

/// <summary>
/// Converts Unix timestamp (seconds since epoch) to DateTime and vice versa.
/// Handles both long (seconds) and nullable DateTime values.
/// </summary>
public class UnixTimestampConverter : DateTimeConverterBase
{
    /// <summary>
    /// Represents the Unix epoch (January 1, 1970, 00:00:00 UTC).
    /// </summary>
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Writes the JSON representation of a DateTime object as a Unix timestamp.
    /// </summary>
    /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
    /// <param name="value">The DateTime value to convert. Can be null.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <exception cref="JsonSerializationException">Thrown when the value is not a DateTime object.</exception>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        if (value is DateTime dateTime)
        {
            var unixTimestamp = (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalSeconds;
            writer.WriteValue(unixTimestamp);
        }
        else
        {
            throw new JsonSerializationException($"Expected DateTime object value, got {value.GetType()}");
        }
    }

    /// <summary>
    /// Reads the JSON representation of a Unix timestamp and converts it to a DateTime object.
    /// Supports integer timestamps, string representations of timestamps, and ISO 8601 date strings.
    /// </summary>
    /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
    /// <param name="objectType">Type of the object being deserialized.</param>
    /// <param name="existingValue">The existing value of object being read. Can be null.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>A DateTime object or null if the value is null and the target type is nullable.</returns>
    /// <exception cref="JsonSerializationException">
    /// Thrown when the JSON token cannot be converted to a DateTime or when a null value is encountered for a non-nullable DateTime.
    /// </exception>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        // Handle null values
        if (reader.TokenType == JsonToken.Null)
        {
            if (objectType == typeof(DateTime?))
                return null;
            
            throw new JsonSerializationException($"Cannot convert null value to {objectType}.");
        }

        // Handle Unix timestamp (long or int)
        if (reader.TokenType == JsonToken.Integer)
        {
            var unixTimestamp = Convert.ToInt64(reader.Value);
            return UnixEpoch.AddSeconds(unixTimestamp);
        }

        // Handle string representation of timestamp
        if (reader.TokenType == JsonToken.String)
        {
            var stringValue = reader.Value?.ToString();
            if (string.IsNullOrEmpty(stringValue))
            {
                if (objectType == typeof(DateTime?))
                    return null;
                
                return DateTime.MinValue;
            }

            if (long.TryParse(stringValue, out var unixTimestamp))
            {
                return UnixEpoch.AddSeconds(unixTimestamp);
            }

            // Try parsing as ISO 8601 date string (fallback)
            if (DateTime.TryParse(stringValue, out var parsedDate))
            {
                return parsedDate;
            }
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing Unix timestamp.");
    }
}
