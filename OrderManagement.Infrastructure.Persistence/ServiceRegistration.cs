using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Factories;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Strategies;
using OrderManagement.Domain.Strategies.Base;
using OrderManagement.Infrastructure.Persistence.Contexts;
using OrderManagement.Infrastructure.Persistence.Repositories;

namespace OrderManagement.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                      configuration.GetConnectionString("DefaultConnection")));
            }
            services.AddScoped<IUnitOfWork>(
              provider => provider.GetRequiredService<ApplicationDbContext>());
            //Sample Code
            #region Repositories

            services.AddTransient<IDiscountStrategyFactory, DiscountStrategyFactory>();

            services.AddTransient<IDiscountStrategy, PremiumCustomerDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, FixedDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, PercentageDiscountStrategy>();

            services.AddTransient<IOrderFactory, OrderFactory>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            //services.AddTransient<IEmployeeRepositoryAsync, EmployeeRepositoryAsync>();

            #endregion Repositories
        }
    }
}