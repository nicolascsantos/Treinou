using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Threading.RateLimiting;
using Treinou.API.Configurations.Policies;
using Treinou.API.Filters;
using Treinou.Infraestructure.Identity;

namespace Treinou.API.Configurations
{
    public static class ControllersConfiguration
    {
        public static IServiceCollection AddAndConfigureControllers(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 4,
                            Window = TimeSpan.FromSeconds(10),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });
            services.AddControllers(options =>
                options.Filters.Add(typeof(APIGlobalExceptionFilter)))
                    .AddJsonOptions(x => x.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCasePolicy());
            services.AddDocumentation();
            return services;
        }

        private static IServiceCollection AddDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var bearerScheme = new OpenApiSecurityScheme
                {
                    Description = "Informe o token JWT (sem o prefixo 'Bearer')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                options.AddSecurityDefinition("Bearer", bearerScheme);
                options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
                {
                    { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
                });
            });
            return services;
        }

        public static WebApplication UseDocumentation(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                // Swashbuckle 10.x + Microsoft.OpenApi 2.4.1 bug: OpenApiSecuritySchemeReference
                // as a dictionary key serializes as {} instead of {"Bearer":[]}.
                // This middleware patches the generated spec before it reaches Swagger UI.
                app.Use(async (ctx, next) =>
                {
                    if (ctx.Request.Path == "/swagger/v1/swagger.json")
                    {
                        var originalBody = ctx.Response.Body;
                        using var ms = new MemoryStream();
                        ctx.Response.Body = ms;
                        await next(ctx);
                        ms.Seek(0, SeekOrigin.Begin);
                        var json = await new StreamReader(ms).ReadToEndAsync();
                        var patched = System.Text.RegularExpressions.Regex.Replace(
                            json,
                            @"""security"":\s*\[\s*\{\s*\}\s*\]",
                            @"""security"":[{""Bearer"":[]}]");
                        ctx.Response.Body = originalBody;
                        var bytes = System.Text.Encoding.UTF8.GetBytes(patched);
                        ctx.Response.ContentLength = bytes.Length;
                        await ctx.Response.Body.WriteAsync(bytes);
                    }
                    else
                    {
                        await next(ctx);
                    }
                });

                app.UseSwagger();
                app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "swagger"));
            }
            return app;
        }
    }
}
