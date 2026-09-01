using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;

public class AdminRepository : BaseRepository<Admin, int>
{
    public AdminRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Admin?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(
                x => x.Email == email.ToLower(),
                cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return !await DbSet
            .AnyAsync(
                x => x.Email == email.ToLower(),
                cancellationToken);
    }
}

