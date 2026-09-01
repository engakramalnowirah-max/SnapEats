using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SnapEats.Application.Common.Models;
using SnapEats.Domain.Exceptions;
using SnapEats.Infrastructure.Identity;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly CustomerRepository _customerRepository;
    private readonly AdminRepository _adminRepository;
    private readonly PasswordService _passwordService;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(
        CustomerRepository customerRepository,
        AdminRepository adminRepository,
        PasswordService passwordService,
        IConfiguration configuration)
    {
        _customerRepository = customerRepository;
        _adminRepository = adminRepository;
        _passwordService = passwordService;
        _configuration = configuration;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (request.Role == "Customer")
        {
            var customer = await _customerRepository.GetByEmailAsync(request.Email, cancellationToken)
                ?? throw new UnauthorizedDomainAccessException("Invalid email or password.");

            if (!_passwordService.VerifyPassword(request.Password, customer.PasswordHash))
                throw new UnauthorizedDomainAccessException("Invalid email or password.");

            var expiresAt = DateTime.UtcNow.AddHours(24);
            var token = GenerateJwtToken(customer.Email, customer.FullName, "Customer", customer.CustomerId);

            return new AuthResult(
                token,
                string.Empty,
                expiresAt,
                "Customer",
                customer.FullName,
                customer.Email,
                customer.CustomerId);
        }
        else
        {
            var admin = await _adminRepository.GetByEmailAsync(request.Email, cancellationToken)
                ?? throw new UnauthorizedDomainAccessException("Invalid email or password.");

            if (!_passwordService.VerifyPassword(request.Password, admin.PasswordHash))
                throw new UnauthorizedDomainAccessException("Invalid email or password.");

            var expiresAt = DateTime.UtcNow.AddHours(24);
            var token = GenerateJwtToken(admin.Email, admin.FullName, "Admin");

            return new AuthResult(
                token,
                string.Empty,
                expiresAt,
                "Admin",
                admin.FullName,
                admin.Email);
        }
    }

    private string GenerateJwtToken(string email, string fullName, string role, int customerId = 0)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"] ?? "SnapEatsSuperSecretKeyForJwtTokenAuthentication2026!";
        var issuer = jwtSettings["Issuer"] ?? "SnapEatsAPI";
        var audience = jwtSettings["Audience"] ?? "SnapEatsClients";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, email),
            new Claim(ClaimTypes.Name, fullName),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

        if (role == "Customer" && customerId > 0)
        {
            claims.Add(new Claim("CustomerId", customerId.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

