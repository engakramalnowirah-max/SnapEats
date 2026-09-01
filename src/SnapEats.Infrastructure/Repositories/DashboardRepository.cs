using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;


public sealed class DashboardRepository
{
    private readonly ApplicationDbContext _context;


    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<VwDashboard?> GetDashboardAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.VwDashboards
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
}