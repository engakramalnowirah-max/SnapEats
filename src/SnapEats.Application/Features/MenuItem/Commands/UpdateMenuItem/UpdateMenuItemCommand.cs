using MediatR;

namespace SnapEats.Application.Features.MenuItem.Commands.UpdateMenuItem;

public sealed record UpdateMenuItemCommand : IRequest
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsAvailable { get; init; }
}

