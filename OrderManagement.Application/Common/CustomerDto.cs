using System;

namespace OrderManagement.Application.Common;

public record CustomerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string Phone,
    DateTime CreatedAt
);
