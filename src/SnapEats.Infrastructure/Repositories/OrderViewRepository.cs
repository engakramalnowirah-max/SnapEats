using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;


public sealed class OrderViewRepository
{
    private readonly ApplicationDbContext _context;


    public OrderViewRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<List<VwOrder>> GetOrdersAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.VwOrders
            .AsNoTracking()
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync(cancellationToken);
    }
}