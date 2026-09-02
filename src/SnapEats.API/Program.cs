using System.Text.Json;
using SnapEats.API.Extensions;
using SnapEats.API.Hubs;
using SnapEats.API.Middleware;
using SnapEats.Application;
using SnapEats.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/snapeats-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddOpenApi();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCorsPolicy();
builder.Services.AddSignalR();
builder.Services.AddJwtAuthentication(builder.Configuration);

// Add layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Register SignalR notification service (uses IHubContext<OrderHub>)
builder.Services.AddScoped<SnapEats.Domain.Interfaces.IRealTimeNotificationService, SnapEats.API.Services.SignalRNotificationService>();

// Configure API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SnapEats API v1");
    c.RoutePrefix = "swagger";
});

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

//app.Use(async (context, next) =>
//{
//    if (string.Equals(context.Request.Method, "OPTIONS", StringComparison.OrdinalIgnoreCase))
//    {
//        context.Response.StatusCode = StatusCodes.Status200OK;
//        context.Response.Headers["Access-Control-Allow-Origin"] = "*";
//        context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
//        context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization, X-Requested-With, Accept, Cache-Control";
//        context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
//        await context.Response.StartAsync();
//        return;
//    }
//    await next();
//});

app.UseCors("SnapEatsCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<OrderHub>("/hubs/order").AllowAnonymous();

try
{
    Log.Information("Starting SnapEats API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "SnapEats API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


