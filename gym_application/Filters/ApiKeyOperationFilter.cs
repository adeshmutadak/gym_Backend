using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace gym_application.Filters
{
    /// <summary>Shows the X-API-KEY box in Swagger only on actions marked [ApiKey].</summary>
    public sealed class ApiKeyOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var needsKey = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<ApiKeyAttribute>()
                .Any();

            if (!needsKey) return;

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}
