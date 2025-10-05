using ByteForge.Toolkit;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Telecom.Interop
{
    /// <summary>
    /// Provides a base class for interop implementations, offering common URL building and API call functionality.
    /// </summary>
    public abstract class InteropBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="InteropBase"/> class.
        /// </summary>
        protected InteropBase() : this(WebApi.Default) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteropBase"/> class with the specified API instance.
        /// </summary>
        /// <param name="api">The <see cref="IWebApi"/> instance used to interact with the web API. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="api"/> is <see langword="null"/>.</exception>
        protected InteropBase(IWebApi api)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
        }

        /// <summary>
        /// Provides access to the underlying web API client used for making HTTP requests.
        /// </summary>
        /// <remarks>
        /// This field is intended to be used by derived classes to interact with the web API. 
        /// It should be initialized before use and is typically set to an implementation of <see cref="IWebApi"/>.
        /// </remarks>
        protected IWebApi Api;

        /// <summary>
        /// Gets the base URL for the API.
        /// </summary>
        public abstract string BaseUrl { get; }

        /// <summary>
        /// Builds a complete API URL by combining the base URL, endpoint, and optional parameters.
        /// </summary>
        /// <param name="endpoint">The API endpoint to append to the base URL.</param>
        /// <param name="parameters">Optional parameters to append to the URL.</param>
        /// <returns>The combined API URL as a string.</returns>
        protected virtual string BuildApiUrl(string endpoint, params string[] parameters)
        {
            var baseUrl = Url.Combine(BaseUrl, endpoint);
            if (parameters != null && parameters.Length > 0)
                baseUrl = Url.Combine(baseUrl, parameters);
            return baseUrl;
        }

        /// <summary>
        /// Makes a safe API call using the appropriate version's method.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="method">The HTTP method to use.</param>
        /// <param name="url">The API URL.</param>
        /// <param name="body">The request body.</param>
        /// <param name="result">The result of the API call.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        protected bool TryCallApi<T>(Method method, string url, object body, out T result)
        {
            try
            {
                result = CallApi<T>(method, url, body);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Makes a safe API call with query parameters.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="method">The HTTP method to use.</param>
        /// <param name="url">The API URL.</param>
        /// <param name="queryParams">Query parameters to add to the request.</param>
        /// <param name="body">The request body.</param>
        /// <param name="result">The result of the API call.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        protected bool TryCallApiWithParams<T>(Method method, string url, Dictionary<string, string> queryParams, object body, out T result)
        {
            try
            {
                result = CallApiWithParams<T>(method, url, queryParams, body);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Makes an API call using bearer token authentication.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="method">The HTTP method to use.</param>
        /// <param name="url">The API URL.</param>
        /// <param name="body">The request body.</param>
        /// <returns>The result of the API call.</returns>
        /// <exception cref="ArgumentException">Thrown when the URL is null or empty.</exception>
        protected T CallApi<T>(Method method, string url, object body) => CallApiWithParams<T>(method, url, null, body);

        /// <summary>
        /// Makes an API call with query parameters.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="method">The HTTP method to use.</param>
        /// <param name="url">The API URL.</param>
        /// <param name="queryParams">Query parameters to add to the request.</param>
        /// <param name="body">The request body.</param>
        /// <returns>The result of the API call.</returns>
        /// <exception cref="ArgumentException">Thrown when the URL is null or empty.</exception>
        protected T CallApiWithParams<T>(Method method, string url, Dictionary<string, string> queryParams, object body = null)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));

            queryParams ??= new Dictionary<string, string>();
            var request = new RestRequest(url, method);
            AddAuthorizationHeader(request);

            // Add query parameters
            foreach (var param in queryParams)
            {
                if (param.Value != null)
                    request.AddQueryParameter(param.Key, param.Value);
            }

            return WebApi.ExecuteRequest<T>(request, body);
        }

        /// <summary>
        /// Adds an authorization header to the specified <see cref="RestRequest"/>.
        /// Override this method in derived classes to provide custom authentication.
        /// </summary>
        /// <param name="request">The REST request to which the authorization header will be added.</param>
        protected virtual void AddAuthorizationHeader(RestRequest request) { }
    }
}
