using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;

public sealed class CustomerOrderRepository : BaseRepository<CustomerOrder, int>
{
    public CustomerOrderRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<CustomerOrder>> GetOrdersByCustomerAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CustomerOrder>> GetOrdersByStatusAsync(
        string status,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(o => o.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<CustomerOrder?> GetOrderWithItemsAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(
                o => o.OrderId == orderId,
                cancellationToken);
    }
}

