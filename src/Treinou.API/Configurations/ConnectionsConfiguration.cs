using Microsoft.EntityFrameworkCore;
using Treinou.Infraestructure;

namespace Treinou.API.Configurations
{
    public static class ConnectionsConfiguration
    {
        public static IServiceCollection AddAppConnections(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbConnection(configuration);
            return services;
        }

        private static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration configuration) 
        {
            var connectionString = configuration.GetConnectionString("TreinouDb") ?? string.Empty;
            services.AddDbContext<TreinouDbContext>(x => x.UseSqlServer(connectionString));
            return services;
        }
    }
}
