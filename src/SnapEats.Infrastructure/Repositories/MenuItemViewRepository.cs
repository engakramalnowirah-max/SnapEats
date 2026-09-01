using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;


public sealed class MenuItemViewRepository
{
    private readonly ApplicationDbContext _context;


    public MenuItemViewRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<List<VwMenuItem>> GetMenuItemsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.VwMenuItems
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}