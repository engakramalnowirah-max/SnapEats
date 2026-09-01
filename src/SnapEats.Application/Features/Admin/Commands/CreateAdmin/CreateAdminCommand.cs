using MediatR;
using SnapEats.Application.Common.Models;

namespace SnapEats.Application.Features.Admin.Commands.CreateAdmin;

public sealed record CreateAdminCommand : IRequest<AuthResult>
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

