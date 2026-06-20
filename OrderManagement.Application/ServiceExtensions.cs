using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Behaviours;
using OrderManagement.Application.Helpers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using System.Reflection;

namespace OrderManagement.Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IModelHelper, ModelHelper>();
            services.AddScoped<IOrderService, OrderService>();

            services.Decorate<IOrderService,
                LoggingOrderServiceDecorator>();

          
            //services.AddScoped<IMockData, MockData>();
        }
    }
}