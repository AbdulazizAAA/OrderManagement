using OrderManagement.Application.Interfaces;
using System;

namespace OrderManagement.API.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}