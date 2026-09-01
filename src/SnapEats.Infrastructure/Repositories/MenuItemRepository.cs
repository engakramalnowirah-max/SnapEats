using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;

public sealed class MenuItemRepository : BaseRepository<MenuItem, int>
{
    public MenuItemRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<MenuItem>> GetMenuItemsByCategoryAsync(
        int categoryId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(m => m.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<MenuItem>> SearchMenuItemsAsync(
        string searchTerm,
        CancellationToken cancellationToken = default)
    {
        var search = searchTerm.ToLower();

        return await DbSet
            .AsNoTracking()
            .Where(m =>
                m.Name.ToLower().Contains(search)
                ||
                (m.Description != null &&
                 m.Description.ToLower().Contains(search)))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<MenuItem>> GetAvailableMenuItemsAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(m => m.IsAvailable == true)
            .ToListAsync(cancellationToken);
    }
}

