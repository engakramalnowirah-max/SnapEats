namespace SnapEats.Application.Features.Admin.Commands.DeleteAdmin;

using MediatR;
using Microsoft.EntityFrameworkCore;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Persistence;

public sealed record DeleteAdminCommand : IRequest
{
    public int Id { get; init; }
}

public sealed class DeleteAdminCommandHandler : IRequestHandler<DeleteAdminCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteAdminCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteAdminCommand request, CancellationToken cancellationToken)
    {
        var admin = await _context.Admins
            .FirstOrDefaultAsync(a => a.AdminId == request.Id, cancellationToken)
            ?? throw new UnauthorizedDomainAccessException($"Admin with Id '{request.Id}' not found.");

        _context.Admins.Remove(admin);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
