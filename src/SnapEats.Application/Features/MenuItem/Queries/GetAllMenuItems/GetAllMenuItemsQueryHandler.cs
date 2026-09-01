using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Application.Common.Models;
using SnapEats.Application.Features.MenuItem.DTOs;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.MenuItem.Queries.GetAllMenuItems;

public sealed class GetAllMenuItemsQueryHandler : IRequestHandler<GetAllMenuItemsQuery, PagedResult<MenuItemDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllMenuItemsQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<MenuItemDto>> Handle(GetAllMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MenuItems
            .AsNoTracking()
            .Include(m => m.Category)
            .AsQueryable();

        // Filter by category
        if (request.CategoryId.HasValue)
            query = query.Where(m => m.CategoryId == request.CategoryId.Value);

        // Search
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(m => m.Name.ToLower().Contains(search)
                || (m.Description != null && m.Description.ToLower().Contains(search)));
        }

        // Sort
        query = (request.SortBy?.ToLower()) switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(m => m.Name) : query.OrderBy(m => m.Name),
            "price" => request.SortDescending ? query.OrderByDescending(m => m.Price) : query.OrderBy(m => m.Price),
            _ => query.OrderBy(m => m.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = _mapper.Map<List<MenuItemDto>>(items);
        return PagedResult<MenuItemDto>.Create(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}

