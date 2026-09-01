using MediatR;

namespace SnapEats.Application.Features.Category.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand : IRequest
{
    public int Id { get; init; }
}

