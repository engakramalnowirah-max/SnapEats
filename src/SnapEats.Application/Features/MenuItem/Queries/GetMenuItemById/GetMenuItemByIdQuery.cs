using MediatR;
using SnapEats.Application.Features.MenuItem.DTOs;

namespace SnapEats.Application.Features.MenuItem.Queries.GetMenuItemById;

public sealed record GetMenuItemByIdQuery : IRequest<MenuItemDto>
{
    public int Id { get; init; }
}

