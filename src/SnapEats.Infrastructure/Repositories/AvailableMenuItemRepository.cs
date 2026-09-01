using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;


public sealed class AvailableMenuItemRepository
{
    private readonly ApplicationDbContext _context;


    public AvailableMenuItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<List<VwAvailableMenuItem>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.VwAvailableMenuItems
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }



    public async Task<VwAvailableMenuItem?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.VwAvailableMenuItems
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.MenuItemId == id,
                cancellationToken);
    }
}