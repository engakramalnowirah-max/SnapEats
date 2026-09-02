using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapEats.AdminMVC.Services;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly CategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(CategoryService categoryService, ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, string? search = null)
    {
        var result = await _categoryService.GetCategoriesAsync(page, 10, search);
        ViewBag.Search = search;
        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateCategoryViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var (success, error) = await _categoryService.CreateCategoryAsync(model);
        if (success)
        {
            TempData["Success"] = "تم إنشاء التصنيف بنجاح";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, error ?? "فشل إنشاء التصنيف");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            TempData["Error"] = "التصنيف غير موجود";
            return RedirectToAction(nameof(Index));
        }

        var model = new EditCategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditCategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var (success, error) = await _categoryService.UpdateCategoryAsync(model);
        if (success)
        {
            TempData["Success"] = "تم تحديث التصنيف بنجاح";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError(string.Empty, error ?? "فشل تحديث التصنيف");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _categoryService.DeleteCategoryAsync(id);
        if (success)
            TempData["Success"] = "تم حذف التصنيف بنجاح";
        else
            TempData["Error"] = error ?? "فشل حذف التصنيف";

        return RedirectToAction(nameof(Index));
    }
}

