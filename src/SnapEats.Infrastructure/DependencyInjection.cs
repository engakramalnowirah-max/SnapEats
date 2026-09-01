using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnapEats.Infrastructure.Identity;
using SnapEats.Infrastructure.Persistence;
using SnapEats.Infrastructure.Repositories;

namespace SnapEats.Infrastructure;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database Context
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            );
        });

        // Password Hashing Service
        services.AddScoped<PasswordService>();


        // SignalR notification service is registered in API layer (uses IHubContext<OrderHub>)

        // Table Repositories
        services.AddScoped<AdminRepository>();
        services.AddScoped<CustomerRepository>();
        services.AddScoped<CategoryRepository>();
        services.AddScoped<MenuItemRepository>();
        services.AddScoped<OrderRepository>();
        services.AddScoped<CustomerOrderRepository>();

        // View Repositories (Read Only)
        services.AddScoped<DashboardRepository>();
        services.AddScoped<AvailableMenuItemRepository>();
        services.AddScoped<CategoryViewRepository>();
        services.AddScoped<MenuItemViewRepository>();
        services.AddScoped<OrderViewRepository>();
        services.AddScoped<OrderDetailRepository>();
        services.AddScoped<OrderInvoiceRepository>();

        return services;
    }
}


