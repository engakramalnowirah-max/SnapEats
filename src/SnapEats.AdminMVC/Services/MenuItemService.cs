using SnapEats.AdminMVC.HttpClients;
using SnapEats.AdminMVC.ViewModels;

namespace SnapEats.AdminMVC.Services;

public class MenuItemService
{
    private readonly SnapEatsApiClient _apiClient;
    private readonly ILogger<MenuItemService> _logger;

    public MenuItemService(SnapEatsApiClient apiClient, ILogger<MenuItemService> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task<PagedResultViewModel<MenuItemViewModel>> GetMenuItemsAsync(int page = 1, int pageSize = 10, string? search = null, int? categoryId = null)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["pageNumber"] = page.ToString(),
            ["pageSize"] = pageSize.ToString()
        };

        if (!string.IsNullOrEmpty(search))
            queryParams["searchTerm"] = search;

        if (categoryId.HasValue)
            queryParams["categoryId"] = categoryId.Value.ToString();

        var response = await _apiClient.GetAsync<PagedResultViewModel<MenuItemViewModel>>("/MenuItem", queryParams);
        return response.Data ?? new PagedResultViewModel<MenuItemViewModel>();
    }

    public async Task<MenuItemViewModel?> GetMenuItemByIdAsync(int id)
    {
        var response = await _apiClient.GetAsync<MenuItemViewModel>($"/MenuItem/{id}");
        return response.Data;
    }

    public async Task<(bool Success, string? Error)> CreateMenuItemAsync(CreateMenuItemViewModel model)
    {
        var payload = new
        {
            categoryId = model.CategoryId,
            name = model.Name,
            description = model.Description,
            price = model.Price,
            imageUrl = model.ImageUrl,
            isAvailable = model.IsAvailable
        };
        var response = await _apiClient.PostAsync<object>("/MenuItem", payload);
        return (response.IsSuccess, response.ErrorMessage);
    }

    public async Task<(bool Success, string? Error)> UpdateMenuItemAsync(EditMenuItemViewModel model)
    {
        var payload = new
        {
            id = model.Id,
            name = model.Name,
            description = model.Description,
            price = model.Price,
            imageUrl = model.ImageUrl,
            isAvailable = model.IsAvailable
        };
        var response = await _apiClient.PutAsync<object>($"/MenuItem/{model.Id}", payload);
        return (response.IsSuccess, response.ErrorMessage);
    }

    public async Task<(bool Success, string? Error)> DeleteMenuItemAsync(int id)
    {
        var response = await _apiClient.DeleteAsync<object>($"/MenuItem/{id}");
        return (response.IsSuccess, response.ErrorMessage);
    }
}

