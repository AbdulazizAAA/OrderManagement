using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Domain.Interfaces;
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
            services.AddScoped<IUnitOfWork>(
              provider => provider.GetRequiredService<ApplicationDbContext>());
            //Sample Code
            #region Repositories


            //services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            //services.AddTransient<IPositionRepositoryAsync, PositionRepositoryAsync>();
            //services.AddTransient<IEmployeeRepositoryAsync, EmployeeRepositoryAsync>();

            #endregion Repositories
        }
    }
}