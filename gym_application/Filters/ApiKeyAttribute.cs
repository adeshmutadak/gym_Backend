using System.Net;
using System.Security.Cryptography;
using System.Text;
using Common.Options;
using CommonLayer.CommonResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace gym_application.Filters
{
    /// <summary>
    /// Put [ApiKey] on an action to require the X-API-KEY header.
    /// Use it only on Admin endpoints; trainer and client endpoints
    /// are protected by the bearer token alone.
    /// </summary>
    public sealed class ApiKeyAttribute : TypeFilterAttribute
    {
        public ApiKeyAttribute() : base(typeof(ApiKeyAuthorizationFilter)) { }
    }

    public sealed class ApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private readonly ApiKeyOptions _options;

        public ApiKeyAuthorizationFilter(IOptions<ApiKeyOptions> options)
        {
            _options = options.Value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headerName = string.IsNullOrWhiteSpace(_options.HeaderName)
                ? "X-API-KEY"
                : _options.HeaderName;

            if (!context.HttpContext.Request.Headers.TryGetValue(headerName, out var provided)
                || string.IsNullOrWhiteSpace(provided))
            {
                context.Result = Deny($"{headerName} header is missing");
                return;
            }

            if (!Matches(provided.ToString(), _options.Key))
            {
                context.Result = Deny("Invalid API key");
            }
        }

        /// <summary>Length-independent comparison so the check cannot leak the key by timing.</summary>
        private static bool Matches(string provided, string expected)
        {
            if (string.IsNullOrEmpty(expected)) return false;

            var a = Encoding.UTF8.GetBytes(provided);
            var b = Encoding.UTF8.GetBytes(expected);

            return a.Length == b.Length && CryptographicOperations.FixedTimeEquals(a, b);
        }

        private static JsonResult Deny(string message) => new(new BaseResponse
        {
            Success = false,
            Message = message,
            HttpStatusCode = HttpStatusCode.Unauthorized
        })
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        };
    }
}
