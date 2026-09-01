using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;

public sealed class CategoryRepository : BaseRepository<Category, int>
{
    public CategoryRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<List<Category>> GetCategoriesWithMenuItemsAsync(
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(c => c.MenuItems)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsNameUniqueAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return !await DbSet
            .AnyAsync(
                c => c.Name.ToLower() == name.ToLower(),
                cancellationToken);
    }
}

