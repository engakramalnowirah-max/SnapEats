using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using SnapEats.AdminMVC.HttpClients;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Services;

public class AuthService
{
    private readonly SnapEatsApiClient _apiClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;
    private string? _jwtToken;

    public AuthService(
        SnapEatsApiClient apiClient,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger)
    {
        _apiClient = apiClient;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<(bool Success, string? Error)> LoginAsync(LoginViewModel model)
    {
        try
        {
            var payload = new
            {
                email = model.Email,
                password = model.Password,
                role = "Admin"
            };

            var response = await _apiClient.PostAsync<AuthResponse>("/Auth/login", payload);

            if (!response.IsSuccess || response.Data == null)
                return (false, response.ErrorMessage ?? "فشل تسجيل الدخول");

            var authData = response.Data;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, model.Email),
                new Claim(ClaimTypes.Name, authData.FullName),
                new Claim(ClaimTypes.Email, authData.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                IssuedUtc = DateTime.UtcNow
            };

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }

            _jwtToken = authData.Token;

            _logger.LogInformation("Admin user {Email} logged in successfully", model.Email);
            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for {Email}", model.Email);
            return (false, "حدث خطأ أثناء تسجيل الدخول");
        }
    }

    public async Task LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        _jwtToken = null;
    }

    public bool IsAuthenticated()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    public string? GetCurrentUserRole()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
    }

    public string? GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    }

    public string? GetCurrentUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
    }

    public string? GetJwtToken()
    {
        return _jwtToken;
    }
}

public class AuthResponse
{
    [JsonProperty("token")]
    public string Token { get; set; } = string.Empty;

    [JsonProperty("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;

    [JsonProperty("expiresAt")]
    public DateTime ExpiresAt { get; set; }

    [JsonProperty("role")]
    public string Role { get; set; } = string.Empty;

    [JsonProperty("fullName")]
    public string FullName { get; set; } = string.Empty;

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;
}

