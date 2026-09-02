using SnapEats.AdminMVC.HttpClients;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Services;

public class CategoryService
{
    private readonly SnapEatsApiClient _apiClient;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(SnapEatsApiClient apiClient, ILogger<CategoryService> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<PagedResultViewModel<CategoryViewModel>> GetCategoriesAsync(int page = 1, int pageSize = 10, string? search = null)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["pageNumber"] = page.ToString(),
            ["pageSize"] = pageSize.ToString()
        };

        if (!string.IsNullOrEmpty(search))
            queryParams["searchTerm"] = search;

        var response = await _apiClient.GetAsync<PagedResultViewModel<CategoryViewModel>>("/Category", queryParams);
        return response.Data ?? new PagedResultViewModel<CategoryViewModel>();
    }

    public async Task<CategoryViewModel?> GetCategoryByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<CategoryViewModel>($"/Category/{id}");
        return response.Data;
    }

    public async Task<(bool Success, string? Error)> CreateCategoryAsync(CreateCategoryViewModel model)
    {
        var payload = new { name = model.Name, description = model.Description };
        var response = await _apiClient.PostAsync<object>("/Category", payload);
        return (response.IsSuccess, response.ErrorMessage);
    }

    public async Task<(bool Success, string? Error)> UpdateCategoryAsync(EditCategoryViewModel model)
    {
        var payload = new { id = model.Id, name = model.Name, description = model.Description };
        var response = await _apiClient.PutAsync<object>($"/Category/{model.Id}", payload);
        return (response.IsSuccess, response.ErrorMessage);
    }

    public async Task<(bool Success, string? Error)> DeleteCategoryAsync(int id)
    {
        var response = await _apiClient.DeleteAsync<object>($"/Category/{id}");
        return (response.IsSuccess, response.ErrorMessage);
    }
}

