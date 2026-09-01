namespace SnapEats.Application.Common.Models;

public sealed record AuthResult(
    string Token,
    string RefreshToken,
    DateTime ExpiresAt,
    string Role,
    string FullName,
    string Email,
    int CustomerId = 0);

