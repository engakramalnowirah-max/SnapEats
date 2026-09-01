using AutoMapper;
using SnapEats.Application.Common.Mappings;
using InfraEntities = SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Application.Features.Admin.DTOs;

public sealed record AdminDto : IMapFrom<InfraEntities.Admin>
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InfraEntities.Admin, AdminDto>();
    }
}

