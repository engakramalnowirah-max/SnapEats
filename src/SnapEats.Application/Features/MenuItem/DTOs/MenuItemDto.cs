using AutoMapper;
using SnapEats.Application.Common.Mappings;
using InfraEntities = SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Application.Features.MenuItem.DTOs;

public sealed record MenuItemDto : IMapFrom<InfraEntities.MenuItem>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsAvailable { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InfraEntities.MenuItem, MenuItemDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.MenuItemId))
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty));
    }
}

public sealed record CreateMenuItemRequest
{
    public int CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsAvailable { get; init; } = true;
}

public sealed record UpdateMenuItemRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsAvailable { get; init; }
}

