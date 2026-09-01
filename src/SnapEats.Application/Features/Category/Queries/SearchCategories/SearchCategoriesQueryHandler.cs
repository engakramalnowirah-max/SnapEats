using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Application.Features.Category.DTOs;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.Category.Queries.SearchCategories;

public sealed class SearchCategoriesQueryHandler : IRequestHandler<SearchCategoriesQuery, IReadOnlyList<CategoryDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchCategoriesQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<CategoryDto>> Handle(SearchCategoriesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var allCategories = await _context.Categories
                .AsNoTracking()
                .Include(c => c.MenuItems)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<CategoryDto>>(allCategories);
        }

        var search = request.SearchTerm.ToLower();
        var categories = await _context.Categories
            .AsNoTracking()
            .Include(c => c.MenuItems)
            .Where(c => c.Name.ToLower().Contains(search)
                || (c.Description != null && c.Description.ToLower().Contains(search)))
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CategoryDto>>(categories);
    }
}

