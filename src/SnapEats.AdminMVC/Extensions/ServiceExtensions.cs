using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using SnapEats.AdminMVC.HttpClients;
using SnapEats.AdminMVC.Services;
using System.Globalization;

namespace SnapEats.AdminMVC.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register HttpClient
        services.AddHttpClient<SnapEatsApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ApiSettings:BaseUrl"]!);
            client.Timeout = TimeSpan.FromSeconds(configuration.GetValue<int>("ApiSettings:TimeoutSeconds", 30));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        // Register Services
        services.AddScoped<AuthService>();
        services.AddScoped<AdminService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<MenuItemService>();
        services.AddScoped<OrderService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<DashboardService>();


        // Register HttpContextAccessor
        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Admin/Auth/Login";
                options.LogoutPath = "/Admin/Auth/Logout";
                options.AccessDeniedPath = "/Admin/Auth/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());
        });

        return services;
    }

    public static IServiceCollection AddArabicLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new("ar-YE"),
                new("en-US")
            };

            options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("ar-SA");

            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;

            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(
                new Microsoft.AspNetCore.Localization.CookieRequestCultureProvider());
        });

        return services;
    }
}

