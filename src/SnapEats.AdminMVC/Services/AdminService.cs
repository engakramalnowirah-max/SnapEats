namespace SnapEats.AdminMVC.Services;

using SnapEats.AdminMVC.HttpClients;
using SnapEats.AdminMVC.ViewModels;

public class AdminService
{
    private readonly SnapEatsApiClient _apiClient;
    private readonly ILogger<AdminService> _logger;

    public AdminService(SnapEatsApiClient apiClient, ILogger<AdminService> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<PagedResultViewModel<AdminViewModel>> GetAdminsAsync(int page = 1, int pageSize = 10, string? search = null)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["pageNumber"] = page.ToString(),
            ["pageSize"] = pageSize.ToString()
        };

        if (!string.IsNullOrEmpty(search))
            queryParams["searchTerm"] = search;

        var response = await _apiClient.GetAsync<PagedResultViewModel<AdminViewModel>>("/Admin", queryParams);
        return response.Data ?? new PagedResultViewModel<AdminViewModel>();
    }

    public async Task<AdminViewModel?> GetAdminByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<AdminViewModel>($"/Admin/{id}");
        return response.Data;
    }

    public async Task<(bool Success, string? Error)> CreateAdminAsync(CreateAdminViewModel model)
    {
        var payload = new
        {
            fullName = model.FullName,
            email = model.Email,
            password = model.Password
        };

        var response = await _apiClient.PostAsync<object>("/Admin/create", payload);
        return (response.IsSuccess, response.ErrorMessage);
    }

    public async Task<(bool Success, string? Error)> DeleteAdminAsync(int id)
    {
        var response = await _apiClient.DeleteAsync<object>($"/Admin/{id}");
        return (response.IsSuccess, response.ErrorMessage);
    }
}
