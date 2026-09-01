using AutoMapper;
using SnapEats.Application.Common.Mappings;
using InfraEntities = SnapEats.Infrastructure.Persistence.Entities;

namespace SnapEats.Application.Features.CustomerOrder.DTOs;

public sealed record OrderDto : IMapFrom<InfraEntities.CustomerOrder>
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public DateTime OrderDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public int ItemCount { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InfraEntities.CustomerOrder, OrderDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.OrderId))
            .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer != null ? s.Customer.FullName : string.Empty))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.ItemCount, opt => opt.MapFrom(s => s.OrderItems.Count));
    }
}

public sealed record OrderDetailDto : IMapFrom<InfraEntities.CustomerOrder>
{
    public int Id { get; init; }
    public int CustomerId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public DateTime OrderDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public IReadOnlyList<OrderItemDto> Items { get; init; } = [];

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InfraEntities.CustomerOrder, OrderDetailDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.OrderId))
            .ForMember(d => d.CustomerName, opt => opt.MapFrom(s => s.Customer != null ? s.Customer.FullName : string.Empty))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.OrderItems));
    }
}

public sealed record OrderItemDto : IMapFrom<InfraEntities.OrderItem>
{
    public int Id { get; init; }
    public string MenuItemName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal SubTotal { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<InfraEntities.OrderItem, OrderItemDto>()
            .ForMember(d => d.MenuItemName, opt => opt.MapFrom(s => s.MenuItem != null ? s.MenuItem.Name : string.Empty));
    }
}

public sealed record CreateOrderRequest
{
    public int CustomerId { get; init; }
    public List<OrderItemRequest> Items { get; init; } = [];
}

public sealed record OrderItemRequest
{
    public int MenuItemId { get; init; }
    public int Quantity { get; init; }
}

