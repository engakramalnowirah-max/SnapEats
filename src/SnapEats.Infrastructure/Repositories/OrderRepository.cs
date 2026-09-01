using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;

public sealed class OrderRepository : BaseRepository<CustomerOrder, int>
{
    public OrderRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<CustomerOrder>> GetOrdersByCustomerAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CustomerOrder>> GetOrdersByStatusAsync(
        string status,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(o => o.Status == status)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<CustomerOrder?> GetOrderWithItemsAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(
                o => o.OrderId == orderId,
                cancellationToken);
    }
}

