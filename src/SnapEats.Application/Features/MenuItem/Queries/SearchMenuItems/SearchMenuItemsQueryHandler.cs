using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Application.Features.MenuItem.DTOs;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.MenuItem.Queries.SearchMenuItems;

public sealed class SearchMenuItemsQueryHandler : IRequestHandler<SearchMenuItemsQuery, IReadOnlyList<MenuItemDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchMenuItemsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MenuItemDto>> Handle(SearchMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MenuItems
            .AsNoTracking()
            .Include(m => m.Category)
            .AsQueryable();

        if (request.CategoryId.HasValue)
            query = query.Where(m => m.CategoryId == request.CategoryId.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(m => m.Name.ToLower().Contains(search)
                || (m.Description != null && m.Description.ToLower().Contains(search)));
        }

        var items = await query.OrderBy(m => m.Name).ToListAsync(cancellationToken);
        return _mapper.Map<List<MenuItemDto>>(items);
    }
}

