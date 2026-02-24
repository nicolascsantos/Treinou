using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
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
                options => options.UseSqlServer(configuration.GetConnectionString("TreinouDb") ?? string.Empty
            ));
            return services;
        }
    }
}
