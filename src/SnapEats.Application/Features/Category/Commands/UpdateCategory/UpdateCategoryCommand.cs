using MediatR;

namespace SnapEats.Application.Features.Category.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand : IRequest
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

