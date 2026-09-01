using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;


public sealed class OrderDetailRepository
{
    private readonly ApplicationDbContext _context;


    public OrderDetailRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<List<VwOrderDetail>> GetDetailsByOrderIdAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.VwOrderDetails
            .AsNoTracking()
            .Where(x => x.OrderId == orderId)
            .ToListAsync(cancellationToken);
    }
}