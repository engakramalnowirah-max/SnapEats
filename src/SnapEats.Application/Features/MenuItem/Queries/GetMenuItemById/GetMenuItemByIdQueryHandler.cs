using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Application.Features.MenuItem.DTOs;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.MenuItem.Queries.GetMenuItemById;

public sealed class GetMenuItemByIdQueryHandler : IRequestHandler<GetMenuItemByIdQuery, MenuItemDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMenuItemByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MenuItemDto> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
    {
        var menuItem = await _context.MenuItems
            .AsNoTracking()
            .Include(m => m.Category)
            .FirstOrDefaultAsync(m => m.MenuItemId == request.Id, cancellationToken)
            ?? throw new InvalidCategoryException($"Menu item with Id '{request.Id}' not found.");

        return _mapper.Map<MenuItemDto>(menuItem);
    }
}

