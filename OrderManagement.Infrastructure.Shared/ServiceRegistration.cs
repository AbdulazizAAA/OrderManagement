using OrderManagement.Application.Interfaces;
using OrderManagement.API.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagement.API.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IMockService, MockService>();
        }
    }
}