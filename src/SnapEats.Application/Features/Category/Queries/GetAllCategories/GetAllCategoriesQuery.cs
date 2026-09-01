using MediatR;
using SnapEats.Application.Common.Models;
using SnapEats.Application.Features.Category.DTOs;

namespace SnapEats.Application.Features.Category.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery : IRequest<PagedResult<CategoryDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? SearchTerm { get; init; }
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
}

