using Entities;
using SupplierOrders.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;
builder.Services.Configure<AppSettings>(configuration);
builder.Services.ConfigureRepository(configuration);
builder.Services.ConfigureServices();
builder.Services.AddJwtConfiguration(configuration);
builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
