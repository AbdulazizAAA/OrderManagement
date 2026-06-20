using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task AddAsync(Order entity)
    {
        await _context.Orders.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public void Update(Order entity)
    {
        _context.Orders.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(Order entity)
    {
        _context.Orders.Remove(entity);
        _context.SaveChanges();
    }

    public async Task<List<Order>> ListAsync(ISpecification<Order> specification)
    {
        IQueryable<Order> query = _context.Orders.AsQueryable();

        // لو بتستخدم Specification Pattern

        return await query.ToListAsync();
    }
}