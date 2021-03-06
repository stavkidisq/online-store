using OnlineStore.Infrastructure;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<OnlineStoreDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineStoreDbContext")));

builder.Services.AddIdentity<AppUserModel, IdentityRole>(options => options.User.RequireUniqueEmail = true)
    .AddEntityFrameworkStores<OnlineStoreDbContext>().AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Products}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        "products",
        "products/{categorySlug}",
        defaults: new { controller = "Products", action = "ProductsByCategory" });

    endpoints.MapControllerRoute(
        "pages",
        "{slug?}",
        defaults: new { controller = "Pages", action = "Page" });
});

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        SeedDataModel.Initialize(services);
    }
    catch (Exception)
    {
        throw;
    }
}
app.Run();
