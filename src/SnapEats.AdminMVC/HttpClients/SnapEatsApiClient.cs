using System.Net;
using System.Text;
using Newtonsoft.Json;
using SnapEats.AdminMVC.Models;

namespace SnapEats.AdminMVC.HttpClients;

public class SnapEatsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SnapEatsApiClient> _logger;

    public SnapEatsApiClient(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        ILogger<SnapEatsApiClient> logger)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint, Dictionary<string, string>? queryParams = null)
    {
        try
        {
            var url = BuildUrl(endpoint, queryParams);
            _logger.LogInformation("GET {Url}", url);

            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GET {Endpoint}", endpoint);
            return ApiResponse<T>.Failure("حدث خطأ في الاتصال بالخادم");
        }
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data = null)
    {
        try
        {
            var url = BuildUrl(endpoint);
            _logger.LogInformation("POST {Url}", url);

            var content = SerializeContent(data);
            var response = await _httpClient.PostAsync(url, content);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in POST {Endpoint}", endpoint);
            return ApiResponse<T>.Failure("حدث خطأ في الاتصال بالخادم");
        }
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object? data = null)
    {
        try
        {
            var url = BuildUrl(endpoint);
            _logger.LogInformation("PUT {Url}", url);

            var content = SerializeContent(data);
            var response = await _httpClient.PutAsync(url, content);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PUT {Endpoint}", endpoint);
            return ApiResponse<T>.Failure("حدث خطأ في الاتصال بالخادم");
        }
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
    {
        try
        {
            var url = BuildUrl(endpoint);
            _logger.LogInformation("DELETE {Url}", url);

            var response = await _httpClient.DeleteAsync(url);
            return await HandleResponse<T>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DELETE {Endpoint}", endpoint);
            return ApiResponse<T>.Failure("حدث خطأ في الاتصال بالخادم");
        }
    }

    private static string BuildUrl(string endpoint, Dictionary<string, string>? queryParams = null)
    {
        var cleanEndpoint = endpoint.TrimStart('/');

        if (queryParams == null || queryParams.Count == 0)
            return cleanEndpoint;

        var queryString = string.Join("&", queryParams
            .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
            .Select(kvp => $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

        return $"{cleanEndpoint}?{queryString}";
    }

    private static StringContent? SerializeContent(object? data)
    {
        if (data == null) return null;
        var json = JsonConvert.SerializeObject(data,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NoContent)
                return ApiResponse<T>.Success(default!);

            var data = JsonConvert.DeserializeObject<T>(content);
            return ApiResponse<T>.Success(data!);
        }

        var errorResponse = ParseErrorResponse(content, response.StatusCode);

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            return ApiResponse<T>.Failure("ليس لديك صلاحية للوصول إلى هذه الصفحة");
        }

        return ApiResponse<T>.Failure(errorResponse);
    }

    private static string ParseErrorResponse(string content, HttpStatusCode statusCode)
    {
        try
        {
            var error = JsonConvert.DeserializeObject<ApiErrorResponse>(content);
            if (error != null)
            {
                if (error.Errors != null && error.Errors.Count > 0)
                {
                    return string.Join("<br/>", error.Errors
                        .SelectMany(e => e.Value)
                        .Select(e => e));
                }
                return error.Detail ?? error.Title ?? "حدث خطأ غير متوقع";
            }
        }
        catch
        {
            // Ignore parsing errors
        }

        return statusCode switch
        {
            HttpStatusCode.NotFound => "العنصر المطلوب غير موجود",
            HttpStatusCode.BadRequest => "بيانات غير صالحة",
            HttpStatusCode.Conflict => "بيانات مكررة",
            _ => "حدث خطأ غير متوقع"
        };
    }
}

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }

    public static ApiResponse<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static ApiResponse<T> Failure(string error) => new() { IsSuccess = false, ErrorMessage = error };
}

public class ApiErrorResponse
{
    public string? Title { get; set; }
    public int StatusCode { get; set; }
    public string? Detail { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}

