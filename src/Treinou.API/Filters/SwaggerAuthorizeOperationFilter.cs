using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Treinou.API.Filters
{
    public class SwaggerAuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize =
                context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() == true ||
                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (!hasAuthorize) return;

            // operation.Security is cleared in case it has leftovers from a previous broken filter run.
            operation.Security.Clear();

            var doc = context.SchemaRepository.Schemas; // just to check we're in the right context
            _ = doc; // unused — actual scheme comes from global AddSecurityRequirement
        }
    }
}
