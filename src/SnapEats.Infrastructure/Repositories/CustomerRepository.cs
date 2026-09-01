using Microsoft.EntityFrameworkCore;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Infrastructure.Repositories;

public sealed class CustomerRepository : BaseRepository<Customer, int>
{
    public CustomerRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(
                c => c.Email == email.ToLower(),
                cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return !await DbSet
            .AnyAsync(
                c => c.Email == email.ToLower(),
                cancellationToken);
    }
}

