using MediatR;
using SnapEats.Application.Common.Models;

namespace SnapEats.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand : IRequest<AuthResult>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = "Customer";
}

