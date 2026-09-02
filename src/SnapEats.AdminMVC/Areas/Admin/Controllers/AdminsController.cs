namespace SnapEats.AdminMVC.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapEats.AdminMVC.Services;
using SnapEats.AdminMVC.ViewModels;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminsController : Controller
{
    private readonly AdminService _adminService;
    private readonly ILogger<AdminsController> _logger;

    public AdminsController(AdminService adminService, ILogger<AdminsController> logger)
    {
        _adminService = adminService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, string? search = null)
    {
        var result = await _adminService.GetAdminsAsync(page, 10, search);
        ViewBag.Search = search;
        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateAdminViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAdminViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var (success, error) = await _adminService.CreateAdminAsync(model);
        if (success)
        {
            TempData["Success"] = "تم إضافة المسؤول بنجاح";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, error ?? "فشل إضافة المسؤول");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _adminService.DeleteAdminAsync(id);
        if (success)
            TempData["Success"] = "تم حذف حساب المسؤول بنجاح";
        else
            TempData["Error"] = error ?? "فشل حذف المسؤول";

        return RedirectToAction(nameof(Index));
    }
}
