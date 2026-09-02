using SnapEats.AdminMVC.HttpClients;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Services;

public class CustomerService
{
    private readonly SnapEatsApiClient _apiClient;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(SnapEatsApiClient apiClient, ILogger<CustomerService> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<PagedResultViewModel<CustomerViewModel>> GetCustomersAsync(int page = 1, int pageSize = 10, string? search = null)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["pageNumber"] = page.ToString(),
            ["pageSize"] = pageSize.ToString()
        };

        if (!string.IsNullOrEmpty(search))
            queryParams["searchTerm"] = search;

        var response = await _apiClient.GetAsync<PagedResultViewModel<CustomerViewModel>>("/Customer", queryParams);
        return response.Data ?? new PagedResultViewModel<CustomerViewModel>();
    }

    public async Task<CustomerViewModel?> GetCustomerByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<CustomerViewModel>($"/Customer/{id}");
        return response.Data;
    }

    public async Task<(bool Success, string? Error)> UpdateCustomerAsync(int id, string fullName, string phone)
    {
        var payload = new { id, fullName, phone };
        var response = await _apiClient.PutAsync<object>($"/Customer/{id}", payload);
        return (response.IsSuccess, response.ErrorMessage);
    }

    public async Task<(bool Success, string? Error)> DeleteCustomerAsync(int id)
    {
        var response = await _apiClient.DeleteAsync<object>($"/Customer/{id}");
        return (response.IsSuccess, response.ErrorMessage);
    }
}

