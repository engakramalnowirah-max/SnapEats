using Microsoft.AspNetCore.Mvc;
using SnapEats.AdminMVC.Services;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class AuthController : Controller
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (_authService.IsAuthenticated())
            return RedirectToAction("Index", "Dashboard");

        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var (success, error) = await _authService.LoginAsync(model);

        if (success)
            return RedirectToAction("Index", "Dashboard");

        ModelState.AddModelError(string.Empty, error ?? "فشل تسجيل الدخول");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}

