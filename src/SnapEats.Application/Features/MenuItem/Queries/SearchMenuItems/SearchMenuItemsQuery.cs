using MediatR;
using SnapEats.Application.Features.MenuItem.DTOs;

namespace SnapEats.Application.Features.MenuItem.Queries.SearchMenuItems;

public sealed record SearchMenuItemsQuery : IRequest<IReadOnlyList<MenuItemDto>>
{
    public string SearchTerm { get; init; } = string.Empty;
    public int? CategoryId { get; init; }
}

