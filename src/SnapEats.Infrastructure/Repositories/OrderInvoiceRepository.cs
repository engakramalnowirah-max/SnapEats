using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;


public sealed class OrderInvoiceRepository
{
    private readonly ApplicationDbContext _context;


    public OrderInvoiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<List<VwOrderInvoice>> GetInvoiceAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.VwOrderInvoices
            .AsNoTracking()
            .Where(x => x.OrderId == orderId)
            .ToListAsync(cancellationToken);
    }
}