using OrderManagement.Application.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Features.Customer.Orders.Commands;


public class LoggingOrderServiceDecorator
    : IOrderService
{
    private readonly IOrderService inner;
    private readonly ILogger<LoggingOrderServiceDecorator>
        logger;

    public LoggingOrderServiceDecorator(
        IOrderService inner,
        ILogger<LoggingOrderServiceDecorator> logger)
    {
        this.inner = inner;
        this.logger = logger;
    }

    public async Task<Guid> CreateOrderAsync(
        CreateOrderCommand command)
    {
        logger.LogInformation(
            "Creating Order");

        return await inner.CreateOrderAsync(command);
    }

    //public async Task UpdateOrderAsync(
    //    UpdateOrderCommand command)
    //{
    //    logger.LogInformation(
    //        "Updating Order");

    //    await inner.UpdateOrderAsync(command);
    //}

    public async Task DeleteOrderAsync(Guid id)
    {
        logger.LogInformation(
            "Deleting Order {Id}",
            id);

        //await inner.DeleteOrderAsync(id);
    }
}