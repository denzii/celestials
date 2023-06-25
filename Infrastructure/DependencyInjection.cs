using Infrastructure.Interface;
using Infrastructure.Persistence;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraLayer(this IServiceCollection services, bool isTestEnv, string dbConnection, string testDbConnection)
        {
            var connectionString = dbConnection;
            if (isTestEnv)
            {
                connectionString = testDbConnection;
            }
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped(typeof(IRepo<>), typeof(Repo<>));

            return services;
        }

    }
}
