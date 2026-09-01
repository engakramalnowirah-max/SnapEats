using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Application.Features.Customer.DTOs;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Persistence;

namespace SnapEats.Application.Features.Customer.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomerByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == request.Id, cancellationToken)
            ?? throw new InvalidEmailException($"Customer with Id '{request.Id}' not found.");

        return _mapper.Map<CustomerDto>(customer);
    }
}

