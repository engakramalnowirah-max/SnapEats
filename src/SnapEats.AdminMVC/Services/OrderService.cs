using SnapEats.AdminMVC.HttpClients;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Services;

public class OrderService
{
    private readonly SnapEatsApiClient _apiClient;
    private readonly ILogger<OrderService> _logger;

    public OrderService(SnapEatsApiClient apiClient, ILogger<OrderService> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<PagedResultViewModel<OrderViewModel>> GetOrdersAsync(int page = 1, int pageSize = 10, string? status = null)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["pageNumber"] = page.ToString(),
            ["pageSize"] = pageSize.ToString()
        };

        if (!string.IsNullOrEmpty(status))
            queryParams["status"] = status;

        var response = await _apiClient.GetAsync<PagedResultViewModel<OrderViewModel>>("/Order", queryParams);
        return response.Data ?? new PagedResultViewModel<OrderViewModel>();
    }

    public async Task<OrderDetailViewModel?> GetOrderByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<OrderDetailViewModel>($"/Order/{id}");
        return response.Data;
    }

    public async Task<(bool Success, string? Error)> UpdateOrderStatusAsync(int orderId, string status)
    {
        var payload = new { orderId, status };
        var response = await _apiClient.PutAsync<object>($"/Order/{orderId}/status", payload);
        return (response.IsSuccess, response.ErrorMessage);
    }

    public async Task<(bool Success, string? Error)> CancelOrderAsync(int orderId)
    {
        var response = await _apiClient.PutAsync<object>($"/Order/{orderId}/cancel");
        return (response.IsSuccess, response.ErrorMessage);
    }

    public async Task<(bool Success, string? Error)> UpdateOrderAsync(int id, int customerId, string status, decimal totalAmount)
    {
        var payload = new { id, customerId, status, totalAmount };
        var response = await _apiClient.PutAsync<object>($"/Order/{id}/update", payload);
        return (response.IsSuccess, response.ErrorMessage);
    }

    public async Task<(bool Success, string? Error)> DeleteOrderAsync(int id)
    {
        var response = await _apiClient.DeleteAsync<object>($"/Order/{id}");
        return (response.IsSuccess, response.ErrorMessage);
    }
}

