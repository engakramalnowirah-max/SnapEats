namespace SnapEats.Application.Features.Admin.Queries.GetAdminById;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Application.Features.Admin.DTOs;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Persistence;

public sealed record GetAdminByIdQuery : IRequest<AdminDto>
{
    public int Id { get; init; }
}

public sealed class GetAdminByIdQueryHandler : IRequestHandler<GetAdminByIdQuery, AdminDto>
{
    private readonly ApplicationDbContext _context;

    public GetAdminByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdminDto> Handle(GetAdminByIdQuery request, CancellationToken cancellationToken)
    {
        var admin = await _context.Admins.AsNoTracking()
            .FirstOrDefaultAsync(a => a.AdminId == request.Id, cancellationToken)
            ?? throw new UnauthorizedDomainAccessException($"Admin with Id '{request.Id}' not found.");

        return new AdminDto
        {
            Id = admin.AdminId,
            FullName = admin.FullName,
            Email = admin.Email
        };
    }
}
