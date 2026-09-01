using MediatR;
using SnapEats.Application.Features.MenuItem.DTOs;

namespace SnapEats.Application.Features.MenuItem.Commands.CreateMenuItem;

public sealed record CreateMenuItemCommand : IRequest<int>
{
    public int CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsAvailable { get; init; } = true;
}

