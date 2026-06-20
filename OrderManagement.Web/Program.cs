using OrderManagement.Application;
using OrderManagement.Infrastructure.Persistence;
using OrderManagement.Infrastructure.Persistence.Contexts;
using OrderManagement.Infrastructure.Shared;
using OrderManagement.Web.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// =======================
// Serilog
// =======================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

// =======================
// Layers
// =======================
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);

// =======================
// MVC + Razor
// =======================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// =======================
// CORS
// =======================
builder.Services.AddCorsExtension();

var app = builder.Build();

// =======================
// Error Handling
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// =======================
// DB Initialization (FIXED)
// =======================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // ❌ BAD (removed)
    // dbContext.Database.EnsureCreated();

    // ✅ GOOD (optional - only if no migrations yet)
    dbContext.Database.EnsureCreated();
}

// =======================
// Middleware Pipeline
// =======================
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ✅ CORS must be here
app.UseCors();

app.UseAuthorization();

// =======================
// Endpoints
// =======================
app.MapControllers();

app.MapRazorPages();

app.Run();