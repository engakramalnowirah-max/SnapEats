using MediatR;
using SnapEats.Application.Common.Models;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Identity;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.Admin.Commands.CreateAdmin;

public sealed class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, AuthResult>
{
    private readonly AdminRepository _adminRepository;
    private readonly PasswordService _passwordService;

    public CreateAdminCommandHandler(
        AdminRepository adminRepository,
        PasswordService passwordService)
    {
        _adminRepository = adminRepository;
        _passwordService = passwordService;
    }

    public async Task<AuthResult> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
        var emailUnique = await _adminRepository.IsEmailUniqueAsync(request.Email, cancellationToken);
        if (!emailUnique)
            throw new InvalidEmailException(request.Email);

        var passwordHash = _passwordService.HashPassword(request.Password);

        var admin = new SnapEats.Infrastructure.Persistence.Entities.Admin
        {
            FullName = request.FullName,
            Email = request.Email.ToLower(),
            PasswordHash = passwordHash
        };

        await _adminRepository.AddAsync(admin, cancellationToken);

        return new AuthResult(
            string.Empty,
            string.Empty,
            DateTime.UtcNow.AddHours(1),
            "Admin",
            admin.FullName,
            admin.Email);
    }
}
