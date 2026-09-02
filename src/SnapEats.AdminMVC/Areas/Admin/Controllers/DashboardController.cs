using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapEats.AdminMVC.Services;

namespace SnapEats.AdminMVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly DashboardService _dashboardService;
    private readonly AuthService _authService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(DashboardService dashboardService, AuthService authService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _authService = authService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var dashboard = await _dashboardService.GetDashboardAsync();

        var apiBaseUrl = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"] ?? "http://localhost:5000";
        ViewBag.SignalRApiUrl = $"{apiBaseUrl.TrimEnd('/')}/hubs/order";

        return View(dashboard);
    }
}

