using ByteForge.Toolkit;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Telecom.Interop.Models;

namespace Telecom.Interop
{
    /// <summary>
    /// Provides methods for interacting with the BrightPattern Web API.
    /// Implements the <see cref="IWebApi"/> interface to provide HTTP request execution and parameter validation capabilities.
    /// </summary>
    public class WebApi : IWebApi
    {
        /// <summary>
        /// Gets the default singleton instance of the <see cref="WebApi"/> class.
        /// </summary>
        /// <value>A static instance of <see cref="WebApi"/> that can be used for API calls.</value>
        public static readonly IWebApi Default = new WebApi();

        /// <summary>
        /// Gets the JSON serializer settings used for API requests and responses.
        /// </summary>
        /// <value>
        /// A <see cref="JsonSerializerSettings"/> object configured with:
        /// <list type="bullet">
        /// <item>Null and default value handling set to ignore</item>
        /// <item>Missing member handling set to ignore</item>
        /// <item>ISO date format with UTC timezone handling</item>
        /// <item>String enum converter for proper enum serialization</item>
        /// </list>
        /// </value>
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateParseHandling = DateParseHandling.DateTime,
            FloatParseHandling = FloatParseHandling.Double,
            FloatFormatHandling = FloatFormatHandling.DefaultValue,
            Formatting = Formatting.None,
            Converters = new JsonConverter[] { new StringEnumConverter() }
        };

        /// <summary>
        /// Stores the HTTP status code from the last executed request.
        /// </summary>
        private HttpStatusCode _lastStatusCode;

        /// <summary>
        /// Stores the last exception that occurred during API execution.
        /// </summary>
        private Exception _lastException;

        /// <summary>
        /// Gets the last exception that occurred during a Web API call from the default instance.
        /// </summary>
        /// <value>The last exception thrown during request execution, or null if no exception occurred.</value>
        public static Exception LastException
        => Default.LastException;

        /// <summary>
        /// Gets the HTTP status code from the last executed request from the default instance.
        /// </summary>
        /// <value>The HTTP status code returned by the most recent API call.</value>
        public static HttpStatusCode LastStatusCode
        => Default.LastStatusCode;

        /// <summary>
        /// Executes a REST request using the default instance and returns the deserialized response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="request">The REST request to execute.</param>
        /// <returns>The deserialized response of type T.</returns>
        /// <exception cref="WebApiException">Thrown when there is an error executing the Web API call.</exception>
        public static T ExecuteRequest<T>(RestRequest request)
        => Default.ExecuteRequest<T>(request);

        /// <summary>
        /// Executes a REST request with a request body using the default instance and returns the deserialized response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="request">The REST request to execute.</param>
        /// <param name="body">The request body to include in the HTTP request.</param>
        /// <returns>The deserialized response of type T.</returns>
        /// <exception cref="WebApiException">Thrown when there is an error executing the Web API call.</exception>
        public static T ExecuteRequest<T>(RestRequest request, object body)
        => Default.ExecuteRequest<T>(request, body);

        /// <summary>
        /// Validates that a collection is not null and contains at least one element using the default instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to validate.</param>
        /// <param name="parameterName">The name of the parameter being validated, used in exception messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when the collection is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the collection is empty.</exception>
        public static void ValidateCollection<T>(ICollection<T> collection, string parameterName)
        => Default.ValidateCollection<T>(collection, parameterName);

        /// <summary>
        /// Validates that a GUID is not empty using the default instance.
        /// </summary>
        /// <param name="id">The GUID to validate.</param>
        /// <param name="parameterName">The name of the parameter being validated, used in exception messages.</param>
        /// <exception cref="ArgumentException">Thrown when the GUID is empty.</exception>
        public static void ValidateGuid(Guid id, string parameterName)
        => Default.ValidateGuid(id, parameterName);

        /// <summary>
        /// Validates that an object is not null using the default instance.
        /// </summary>
        /// <param name="value">The object to validate.</param>
        /// <param name="parameterName">The name of the parameter being validated, used in exception messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        public static void ValidateNotNull(object value, string parameterName)
        => Default.ValidateNotNull(value, parameterName);

        /// <summary>
        /// Validates that a value is positive (greater than zero) using the default instance.
        /// </summary>
        /// <typeparam name="T">The type of the value, which must be a value type and implement IComparable.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="parameterName">The name of the parameter being validated, used in exception messages.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not positive.</exception>
        public static void ValidatePositive<T>(T value, string parameterName) where T : struct, IComparable<T>
        => Default.ValidatePositive<T>(value, parameterName);

        /// <summary>
        /// Validates that a value falls within the specified range using the default instance.
        /// </summary>
        /// <typeparam name="T">The type of the value, which must implement IComparable.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="minValue">The minimum allowed value (inclusive).</param>
        /// <param name="maxValue">The maximum allowed value (inclusive).</param>
        /// <param name="parameterName">The name of the parameter being validated, used in exception messages.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is outside the specified range.</exception>
        public static void ValidateRange<T>(T value, T minValue, T maxValue, string parameterName) where T : IComparable<T>
        => Default.ValidateRange<T>(value, minValue, maxValue, parameterName);

        /// <summary>
        /// Validates that a string is not null, empty, or consists only of whitespace characters using the default instance.
        /// </summary>
        /// <param name="value">The string to validate.</param>
        /// <param name="parameterName">The name of the parameter being validated, used in exception messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when the string is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the string is empty or whitespace.</exception>
        public static void ValidateString(string value, string parameterName)
        => Default.ValidateString(value, parameterName);

        /// <summary>
        /// Gets or sets the configuration settings for the Web API.
        /// </summary>
        /// <value>A <see cref="WebApiConfig"/> object containing the configuration settings for API communication.</value>
        public WebApiConfig Config = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApi"/> class using configuration from the "WebApi" section.
        /// </summary>
        public WebApi() : this(Configuration.GetSection<WebApiConfig>("WebApi")) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApi"/> class with the specified configuration.
        /// </summary>
        /// <param name="config">The configuration settings for the Web API. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="config"/> is null.</exception>
        public WebApi(WebApiConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Gets the last exception that occurred during a Web API call.
        /// </summary>
        /// <value>The last exception thrown during request execution, or null if no exception occurred.</value>
        Exception IWebApi.LastException { get => _lastException; }

        /// <summary>
        /// Gets the HTTP status code from the last executed request.
        /// </summary>
        /// <value>The HTTP status code returned by the most recent API call.</value>
        HttpStatusCode IWebApi.LastStatusCode { get => _lastStatusCode; }

        /// <summary>
        /// Executes a request to the Web API with a request body and returns the deserialized result.
        /// </summary>
        /// <typeparam name="T">The type of the deserialized result expected from the API call.</typeparam>
        /// <param name="request">The RestRequest object configured with the necessary headers and parameters for the API call.</param>
        /// <param name="body">The request body. If it is a string, it is treated as a JSON object. Otherwise, it is serialized to a JSON object.</param>
        /// <returns>The deserialized result of type <typeparamref name="T"/> from the API call.</returns>
        /// <exception cref="WebApiException">Thrown when there is an error executing the Web API call.</exception>
        /// <remarks>
        /// <para>If the body is a string, it is treated as a JSON object and added directly to the request.</para>
        /// <para>Any other object is serialized to a JSON object using the configured JSON serializer settings.</para>
        /// <para>For binary responses (byte[]), the Accept header is set to "*", otherwise it's set to "application/json".</para>
        /// </remarks>
        T IWebApi.ExecuteRequest<T>(RestRequest request, object body)
        {
            if (typeof(T) == typeof(byte[]))
                request.AddHeader("Accept", "*");
            else
                request.AddHeader("Accept", "application/json");

            if (body is string str && str.Length > 0)
            {
                request.AddHeader("Content-Type", "application/json");
                request.AddBody(str, "text/text");
            }
            else if ((body is null) == false)
            {
                /*
                 * The body is not a string, so it is serialized to a JSON object.
                 * The Content-Type header is set to "application/json".
                 */
                var json = JsonConvert.SerializeObject(body, JsonSettings);

                request.AddHeader("Content-Type", "application/json");
                request.AddBody(json, "text/text");
            }

            return ((IWebApi)this).ExecuteRequest<T>(request);
        }

        /// <summary>
        /// Executes a request to the BrightPattern Web API and returns the result.
        /// </summary>
        /// <typeparam name="T">The type of the result to deserialize the response into.</typeparam>
        /// <param name="request">The REST request to execute, configured with the necessary headers and parameters.</param>
        /// <returns>The deserialized result of type <typeparamref name="T"/> from the API call.</returns>
        /// <exception cref="WebApiException">Thrown when there is an error executing the Web API call, including HTTP errors, deserialization errors, or network issues.</exception>
        /// <remarks>
        /// <para>This method handles both JSON and binary responses based on the requested type.</para>
        /// <para>For binary data (byte[]), the raw response bytes are returned.</para>
        /// <para>For other types, the response is deserialized from JSON using the configured serializer settings.</para>
        /// <para>HTTP client is configured with automatic decompression, no cookies, no auto-redirect, and the security protocol specified in the configuration.</para>
        /// <para>Error responses are parsed and wrapped in a <see cref="WebApiException"/> with detailed error information.</para>
        /// </remarks>
        T IWebApi.ExecuteRequest<T>(RestRequest request)
        {
            _lastException = null;

            using var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                UseCookies = false,
                AllowAutoRedirect = false,
                UseProxy = false,
                Proxy = null,
                SslProtocols = Config.SecurityProtocol.ToSslProtocols(),
            };
            using var client = new HttpClient(handler);
            using var restClient = new RestClient(client);

            /*
             * Execute the request.
             */
            Log.Verbose($"Executing Web API request: {request.Method} {request.Resource}");
            var response = restClient.Execute(request);
            _lastStatusCode = response.StatusCode;
            Log.Verbose($"Web API response: {(int)response.StatusCode} {response.StatusDescription}");

            /*
             * If the response is successful, return the result.
             */
            if (response.IsSuccessStatusCode)
            {
                // Handle binary data (byte array) responses
                if (typeof(T) == typeof(byte[]))
                    return (T)(object)response.RawBytes;

                try
                {
                    // Handle JSON responses
                    return JsonConvert.DeserializeObject<T>(response.Content);
                }
                catch (JsonSerializationException ex)
                {
                    _lastException = new WebApiException("Error deserializing response content.", ex);
                    _lastException.Data.Add("ResponseContent", response.Content);
                    throw _lastException;
                }
            }

            /*
             * Prepare an error message based on the response.
             */
            var baseError = new ErrorResponse
            {
                ErrorType = response.StatusDescription,
                ErrorMessage = response.ErrorMessage ?? "No ErrorMessage in response",
                ResponseContent = response.Content ?? "No Content in response"
            };

            try
            {
                /*
                 * If we can, deserialize the error message from the response.
                 */
                if (response.Content != null && response.Content.Length > 0)
                {
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);
                    if (error != null)
                    {
                        baseError.ErrorType = error.ErrorType ?? baseError.ErrorType;
                        baseError.ErrorMessage = error.ErrorMessage ?? baseError.ErrorMessage;
                        baseError.ResponseContent = error.ResponseContent ?? baseError.ResponseContent;
                    }
                }
            }
            catch (Exception) { }

            /* 
             * Throw an exception with the error message.
             */
            _lastException = new WebApiException($@"({(int)response.StatusCode} {response.StatusDescription}) - '{baseError.ErrorMessage}'", response.ErrorException);
            _lastException.Data.Add("ResponseContent", baseError.ResponseContent);
            throw _lastException;
        }

        /// <summary>
        /// Validates that a collection is not null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to validate.</param>
        /// <param name="parameterName">The name of the parameter for exception messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when the collection is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the collection is empty.</exception>
        void IWebApi.ValidateCollection<T>(ICollection<T> collection, string parameterName)
        {
            if (collection == null)
                throw new ArgumentNullException(parameterName);
            if (collection.Count == 0)
                throw new ArgumentException($"{parameterName} cannot be empty.", parameterName);
        }

        /// <summary>
        /// Validates that a GUID parameter is not empty.
        /// </summary>
        /// <param name="id">The GUID to validate.</param>
        /// <param name="parameterName">The name of the parameter for exception messages.</param>
        /// <exception cref="ArgumentException">Thrown when the GUID is empty.</exception>
        void IWebApi.ValidateGuid(Guid id, string parameterName)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"{parameterName} is empty.", parameterName);
        }

        /// <summary>
        /// Validates that an object parameter is not null.
        /// </summary>
        /// <param name="value">The object to validate.</param>
        /// <param name="parameterName">The name of the parameter for exception messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when the object is null.</exception>
        void IWebApi.ValidateNotNull(object value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Validates that a value is positive.
        /// </summary>
        /// <typeparam name="T">The type of the value, must be a struct and implement IComparable&lt;T&gt;.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="parameterName">The name of the parameter for exception messages.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is not positive.</exception>
        void IWebApi.ValidatePositive<T>(T value, string parameterName)
        {
            var zero = default(T);
            if (value.CompareTo(zero) <= 0)
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be positive.");
        }

        /// <summary>
        /// Validates that a value is within a specified range.
        /// </summary>
        /// <typeparam name="T">The type of the value, must implement IComparable&lt;T&gt;.</typeparam>
        /// <param name="value">The value to validate.</param>
        /// <param name="minValue">The minimum allowed value.</param>
        /// <param name="maxValue">The maximum allowed value.</param>
        /// <param name="parameterName">The name of the parameter for exception messages.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is outside the specified range.</exception>
        void IWebApi.ValidateRange<T>(T value, T minValue, T maxValue, string parameterName)
        {
            if (value.CompareTo(minValue) < 0 || value.CompareTo(maxValue) > 0)
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be between {minValue} and {maxValue}.");
        }

        /// <summary>
        /// Validates that a string parameter is not null or empty.
        /// </summary>
        /// <param name="value">The string to validate.</param>
        /// <param name="parameterName">The name of the parameter for exception messages.</param>
        /// <exception cref="ArgumentException">Thrown when the string is null or empty.</exception>
        void IWebApi.ValidateString(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{parameterName} cannot be null or empty.", parameterName);
        }
    }
}