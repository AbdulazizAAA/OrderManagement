using System;

namespace OrderManagement.Domain.Events;

public record OrderStatusChangedEvent(Guid OrderId, string NewStatus) : IDomainEvent;