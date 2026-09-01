using MediatR;
using SnapEats.Application.Features.Category.DTOs;

namespace SnapEats.Application.Features.Category.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery : IRequest<CategoryDetailDto>
{
    public int Id { get; init; }
}

