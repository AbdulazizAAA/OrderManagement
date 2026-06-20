using System;
namespace OrderManagement.Domain.Events;

public record OrderCreatedEvent(Guid OrderId) : IDomainEvent;
