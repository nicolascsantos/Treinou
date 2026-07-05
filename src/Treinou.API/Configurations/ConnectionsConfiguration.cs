using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Treinou.Infraestructure;
using Treinou.Infraestructure.Identity;

namespace Treinou.API.Configurations
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAppConnections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbConnection(configuration);
            services.AddIdentityAuthentication(configuration);
            return services;
        }

        private static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TreinouDb") ?? string.Empty;
            services.AddDbContext<TreinouDbContext>(x => x.UseSqlServer(connectionString)
                .UseExceptionProcessor());
            return services;
        }

        private static IServiceCollection AddIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("TreinouDb") ?? string.Empty));

            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<AuthDbContext>();

            var jwtSettings = configuration.GetSection("Jwt");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JwtBearer");
                        var hasToken = !string.IsNullOrEmpty(context.Token) || context.Request.Headers.ContainsKey("Authorization");
                        logger.LogInformation("JWT OnMessageReceived — Authorization header present: {HasHeader}, Token extracted: {HasToken}",
                            context.Request.Headers.ContainsKey("Authorization"),
                            !string.IsNullOrEmpty(context.Token));
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JwtBearer");
                        logger.LogError(context.Exception, "JWT authentication failed: {Message}", context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JwtBearer");
                        logger.LogInformation("JWT token validated for subject: {Sub}",
                            context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                            ?? context.Principal?.FindFirst("sub")?.Value);
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("JwtBearer");
                        logger.LogWarning("JWT challenge issued — Error: {Error}, ErrorDescription: {ErrorDescription}",
                            context.Error, context.ErrorDescription);
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
