using System;
using System.Collections.Generic;

namespace OrderManagement.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public ICollection<Order> Orders { get; private set; } = new List<Order>();

    public string FullName => $"{FirstName} {LastName}";

    private Customer() { }

    public Customer(string firstName, string lastName, string email, string phone)
    {
        Id = Guid.NewGuid();
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Phone = phone ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string firstName, string lastName, string email, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }
}