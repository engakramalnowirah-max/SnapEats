using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;


public sealed class CategoryViewRepository
{
    private readonly ApplicationDbContext _context;


    public CategoryViewRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<List<VwCategory>> GetCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.VwCategories
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}