using AutoMapper;
using SnapEats.Application.Common.Mappings;
using InfraEntities = SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Application.Features.Customer.DTOs;

public sealed record CustomerDto : IMapFrom<InfraEntities.Customer>
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InfraEntities.Customer, CustomerDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.CustomerId));
    }
}

public sealed record RegisterCustomerRequest
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public sealed record UpdateCustomerRequest
{
    public string FullName { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}

