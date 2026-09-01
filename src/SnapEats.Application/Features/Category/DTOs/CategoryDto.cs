using AutoMapper;
using SnapEats.Application.Common.Mappings;
using InfraEntities = SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Application.Features.Category.DTOs;

public sealed record CategoryDto : IMapFrom<InfraEntities.Category>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int MenuItemCount { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InfraEntities.Category, CategoryDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CategoryId))
            .ForMember(d => d.MenuItemCount, opt => opt.MapFrom(s => s.MenuItems.Count));
    }
}

public sealed record CategoryDetailDto : IMapFrom<InfraEntities.Category>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public IReadOnlyList<MenuItemDto> MenuItems { get; init; } = [];
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InfraEntities.Category, CategoryDetailDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CategoryId));
    }
}

public sealed record MenuItemDto : IMapFrom<InfraEntities.MenuItem>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsAvailable { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InfraEntities.MenuItem, MenuItemDto>();
    }
}

