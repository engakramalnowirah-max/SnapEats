using Microsoft.AspNetCore.SignalR.Client;
using SnapEats.AdminMVC.HttpClients;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Services;

public class DashboardService
{
    private readonly OrderService _orderService;
    private readonly CategoryService _categoryService;
    private readonly MenuItemService _menuItemService;
    private readonly CustomerService _customerService;
    private readonly ILogger<DashboardService> _logger;
    private HubConnection? _hubConnection;

    public DashboardService(
        OrderService orderService,
        CategoryService categoryService,
        MenuItemService menuItemService,
        CustomerService customerService,
        ILogger<DashboardService> logger)
    {
        _orderService = orderService;
        _categoryService = categoryService;
        _menuItemService = menuItemService;
        _customerService = customerService;
        _logger = logger;
    }

    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        try
        {
            var ordersTask = _orderService.GetOrdersAsync(page: 1, pageSize: 1);
            var categoriesTask = _categoryService.GetCategoriesAsync(page: 1, pageSize: 1);
            var menuItemsTask = _menuItemService.GetMenuItemsAsync(page: 1, pageSize: 1);
            var customersTask = _customerService.GetCustomersAsync(page: 1, pageSize: 1);
            var pendingOrdersTask = _orderService.GetOrdersAsync(page: 1, pageSize: 10, status: "Pending");
            var recentOrdersTask = _orderService.GetOrdersAsync(page: 1, pageSize: 5);

            await Task.WhenAll(ordersTask, categoriesTask, menuItemsTask, customersTask, pendingOrdersTask, recentOrdersTask);

            return new DashboardViewModel
            {
                TotalOrders = ordersTask.Result.TotalCount,
                TotalCategories = categoriesTask.Result.TotalCount,
                TotalMenuItems = menuItemsTask.Result.TotalCount,
                TotalCustomers = customersTask.Result.TotalCount,
                PendingOrders = pendingOrdersTask.Result.TotalCount,
                RecentOrders = recentOrdersTask.Result.Items ?? new List<OrderViewModel>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading dashboard data");
            return new DashboardViewModel();
        }
    }

    public async Task StartSignalRConnectionAsync(string hubUrl, string accessToken, Action onDashboardUpdate)
    {
        if (_hubConnection != null)
            return;

        try
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult<string?>(accessToken);
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On("DashboardUpdated", (object evt) =>
            {
                _logger.LogInformation("DashboardUpdated event received via SignalR at {Time}", DateTime.UtcNow);
                onDashboardUpdate?.Invoke();
            });

            _hubConnection.On("OrderCreated", (object evt) =>
            {
                _logger.LogInformation("OrderCreated event received via SignalR");
                onDashboardUpdate?.Invoke();
            });

            _hubConnection.On("OrderStatusChanged", (object evt) =>
            {
                _logger.LogInformation("OrderStatusChanged event received via SignalR");
                onDashboardUpdate?.Invoke();
            });

            _hubConnection.On("OrderCancelled", (object evt) =>
            {
                _logger.LogInformation("OrderCancelled event received via SignalR");
                onDashboardUpdate?.Invoke();
            });

            await _hubConnection.StartAsync();
            _logger.LogInformation("SignalR connection to OrderHub started successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start SignalR connection to OrderHub");
            _hubConnection = null;
        }
    }

    public async Task StopSignalRConnectionAsync()
    {
        if (_hubConnection != null)
        {
            try
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping SignalR connection");
            }
            finally
            {
                _hubConnection = null;
            }
        }
    }
}

