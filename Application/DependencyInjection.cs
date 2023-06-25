using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppLayer(this IServiceCollection services)
        {
            // this code test doesn't really have any application/business logic which can be added here...
            return services;
        }
    }
}