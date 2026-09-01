using MediatR;
using SnapEats.Application.Common.Models;
using SnapEats.Application.Features.MenuItem.DTOs;

namespace SnapEats.Application.Features.MenuItem.Queries.GetAllMenuItems;

public sealed record GetAllMenuItemsQuery : IRequest<PagedResult<MenuItemDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
    public int? CategoryId { get; init; }
}

