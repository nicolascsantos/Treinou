using Treinou.API.Configurations.Policies;
using Treinou.API.Filters;

namespace Treinou.API.Configurations
{
    public static class ControllersConfiguration
    {
        public static IServiceCollection AddAndConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
                options.Filters.Add(typeof(APIGlobalExceptionFilter)))
                    .AddJsonOptions(x => x.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCasePolicy());
            services.AddDocumentation();
            return services;
        }

        private static IServiceCollection AddDocumentation(this IServiceCollection services)
        {
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static WebApplication UseDocumentation(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(x => x.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));
            }
            return app;
        }
    }
}
