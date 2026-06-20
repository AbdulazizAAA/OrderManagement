using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Domain.Factories;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Strategies;
using OrderManagement.Domain.Strategies.Base;
using OrderManagement.Infrastructure.Persistence.Contexts;

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
         
            //Sample Code
            #region Repositories

            services.AddTransient<IDiscountStrategyResolver, DiscountStrategyResolver>();

            services.AddTransient<IDiscountStrategy, BulkDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, NoDiscountStrategy>();
            services.AddTransient<IDiscountStrategy, PercentageDiscountStrategy>();
            services.AddTransient<IDiscountStrategy,LoyaltyDiscountStrategy>();
            services.AddTransient<IDiscountStrategy,FlatAmountDiscountStrategy>();

            services.AddTransient<IOrderFactory, OrderFactory>();


            #endregion Repositories
        }
    }
}