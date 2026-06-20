using OrderManagement.Domain.Common;
using System;

namespace OrderManagement.Domain.Entities
{
    public class Customer : AuditableBaseEntity
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        private Customer() { }

        public Customer(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
        }
    }
}