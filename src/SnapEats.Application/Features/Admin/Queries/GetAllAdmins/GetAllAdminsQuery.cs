namespace SnapEats.Application.Features.Admin.Queries.GetAllAdmins;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Application.Common.Models;
using SnapEats.Application.Features.Admin.DTOs;
using SnapEats.Infrastructure.Persistence;

public sealed record GetAllAdminsQuery : IRequest<PagedResult<AdminDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
}

public sealed class GetAllAdminsQueryHandler : IRequestHandler<GetAllAdminsQuery, PagedResult<AdminDto>>
{
    private readonly ApplicationDbContext _context;

    public GetAllAdminsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<AdminDto>> Handle(GetAllAdminsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Admins.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(a => a.FullName.ToLower().Contains(term) || a.Email.ToLower().Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        var items = await query
            .OrderByDescending(a => a.AdminId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AdminDto
            {
                Id = a.AdminId,
                FullName = a.FullName,
                Email = a.Email
            })
            .ToListAsync(cancellationToken);

        return PagedResult<AdminDto>.Create(items, totalCount, pageNumber, pageSize);
    }
}
