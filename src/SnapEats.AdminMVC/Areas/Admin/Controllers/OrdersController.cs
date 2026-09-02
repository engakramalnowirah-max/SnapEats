using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnapEats.AdminMVC.Services;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrdersController : Controller
{
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;
    private readonly AuthService _authService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(OrderService orderService, CustomerService customerService, AuthService authService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _customerService = customerService;
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, string? status = null)
    {
        var result = await _orderService.GetOrdersAsync(page, 10, status);
        ViewBag.Status = status;

        var apiBaseUrl = HttpContext.RequestServices.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"] ?? "http://localhost:5000";
        ViewBag.SignalRApiUrl = $"{apiBaseUrl.TrimEnd('/')}/hubs/order";

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            TempData["Error"] = "الطلب غير موجود";
            return RedirectToAction(nameof(Index));
        }

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int orderId, string status)
    {
        var (success, error) = await _orderService.UpdateOrderStatusAsync(orderId, status);
        if (success)
            TempData["Success"] = "تم تحديث حالة الطلب بنجاح";
        else
            TempData["Error"] = error ?? "فشل تحديث حالة الطلب";

        return RedirectToAction(nameof(Details), new { id = orderId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int orderId)
    {
        var (success, error) = await _orderService.CancelOrderAsync(orderId);
        if (success)
            TempData["Success"] = "تم إلغاء الطلب بنجاح";
        else
            TempData["Error"] = error ?? "فشل إلغاء الطلب";

        return RedirectToAction(nameof(Details), new { id = orderId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            TempData["Error"] = "الطلب غير موجود";
            return RedirectToAction(nameof(Index));
        }

        var customers = await _customerService.GetCustomersAsync();
        ViewBag.Customers = customers.Items;

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, int customerId, string status, decimal totalAmount)
    {
        var (success, error) = await _orderService.UpdateOrderAsync(id, customerId, status, totalAmount);
        if (success)
            TempData["Success"] = "تم تحديث الطلب بنجاح";
        else
            TempData["Error"] = error ?? "فشل تحديث الطلب";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _orderService.DeleteOrderAsync(id);
        if (success)
            TempData["Success"] = "تم حذف الطلب بنجاح";
        else
            TempData["Error"] = error ?? "فشل حذف الطلب";

        return RedirectToAction(nameof(Index));
    }
}

