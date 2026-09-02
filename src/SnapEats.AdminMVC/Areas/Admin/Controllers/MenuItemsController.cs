using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapEats.AdminMVC.Services;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class MenuItemsController : Controller
{
    private readonly MenuItemService _menuItemService;
    private readonly CategoryService _categoryService;
    private readonly ILogger<MenuItemsController> _logger;

    public MenuItemsController(
        MenuItemService menuItemService,
        CategoryService categoryService,
        ILogger<MenuItemsController> logger)
    {
        _menuItemService = menuItemService;
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, string? search = null, int? categoryId = null)
    {
        var result = await _menuItemService.GetMenuItemsAsync(page, 10, search, categoryId);
        ViewBag.Search = search;
        ViewBag.CategoryId = categoryId;
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _categoryService.GetCategoriesAsync(1, 100);
        ViewBag.Categories = categories.Items;

        return View(new CreateMenuItemViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMenuItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetCategoriesAsync(1, 100);
            ViewBag.Categories = categories.Items;
            return View(model);
        }

        var (success, error) = await _menuItemService.CreateMenuItemAsync(model);
        if (success)
        {
            TempData["Success"] = "تم إنشاء العنصر بنجاح";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, error ?? "فشل إنشاء العنصر");
        var cats = await _categoryService.GetCategoriesAsync(1, 100);
        ViewBag.Categories = cats.Items;
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
        if (menuItem == null)
        {
            TempData["Error"] = "العنصر غير موجود";
            return RedirectToAction(nameof(Index));
        }

        var categories = await _categoryService.GetCategoriesAsync(1, 100);
        ViewBag.Categories = categories.Items;

        var model = new EditMenuItemViewModel
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Description = menuItem.Description,
            Price = menuItem.Price,
            ImageUrl = menuItem.ImageUrl,
            IsAvailable = menuItem.IsAvailable
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditMenuItemViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetCategoriesAsync(1, 100);
            ViewBag.Categories = categories.Items;
            return View(model);
        }

        var (success, error) = await _menuItemService.UpdateMenuItemAsync(model);
        if (success)
        {
            TempData["Success"] = "تم تحديث العنصر بنجاح";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, error ?? "فشل تحديث العنصر");
        var cats = await _categoryService.GetCategoriesAsync(1, 100);
        ViewBag.Categories = cats.Items;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _menuItemService.DeleteMenuItemAsync(id);
        if (success)
            TempData["Success"] = "تم حذف العنصر بنجاح";
        else
            TempData["Error"] = error ?? "فشل حذف العنصر";

        return RedirectToAction(nameof(Index));
    }
}

