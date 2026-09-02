using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapEats.AdminMVC.Services;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CustomersController : Controller
{
    private readonly CustomerService _customerService;
    private readonly OrderService _orderService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(
        CustomerService customerService,
        OrderService orderService,
        ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, string? search = null)
    {
        var result = await _customerService.GetCustomersAsync(page, 10, search);
        ViewBag.Search = search;
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            TempData["Error"] = "العميل غير موجود";
            return RedirectToAction(nameof(Index));
        }

        var orders = await _orderService.GetOrdersAsync(1, 10);
        ViewBag.CustomerOrders = orders.Items.Where(o => o.CustomerId == id).ToList();

        return View(customer);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
        {
            TempData["Error"] = "العميل غير موجود";
            return RedirectToAction(nameof(Index));
        }

        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string fullName, string phone)
    {
        var (success, error) = await _customerService.UpdateCustomerAsync(id, fullName, phone);
        if (success)
            TempData["Success"] = "تم تحديث بيانات العميل بنجاح";
        else
            TempData["Error"] = error ?? "فشل تحديث بيانات العميل";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _customerService.DeleteCustomerAsync(id);
        if (success)
            TempData["Success"] = "تم حذف العميل بنجاح";
        else
            TempData["Error"] = error ?? "فشل حذف العميل";

        return RedirectToAction(nameof(Index));
    }
}

