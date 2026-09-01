using MediatR;
using SnapEats.Application.Features.Category.DTOs;

namespace SnapEats.Application.Features.Category.Queries.SearchCategories;

public sealed record SearchCategoriesQuery : IRequest<IReadOnlyList<CategoryDto>>
{
    public string SearchTerm { get; init; } = string.Empty;
}

