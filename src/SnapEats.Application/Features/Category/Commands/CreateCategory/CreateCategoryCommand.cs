using MediatR;

namespace SnapEats.Application.Features.Category.Commands.CreateCategory;

public sealed record CreateCategoryCommand : IRequest<int>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}

