using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Application.Features.CustomerOrder.DTOs;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.CustomerOrder.Queries.GetOrderById;

public sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderDetailDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.CustomerOrders
            .AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.OrderId == request.Id, cancellationToken)
            ?? throw new InvalidCategoryException($"Order with Id '{request.Id}' not found.");

        return _mapper.Map<OrderDetailDto>(order);
    }
}

