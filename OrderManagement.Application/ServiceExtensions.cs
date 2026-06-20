using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.API.Application.Behaviours;
using OrderManagement.API.Application.Helpers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using System.Reflection;

namespace OrderManagement.API.Application
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