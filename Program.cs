using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AspnetCoreStarter.Data;
using AspnetCoreStarter.Data.Interfaces;
using AspnetCoreStarter.Data.Interfaces.Musteri;
using AspnetCoreStarter.Data.Interfaces.Bildirim;
using AspnetCoreStarter.Data.Repositories;
using AspnetCoreStarter.Data.Repositories.Musteri;
using AspnetCoreStarter.Data.Repositories.Bildirim;
using AspnetCoreStarter.Services;
using AspnetCoreStarter.Services.Interfaces;
// using AspnetCoreStarter.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // API Controller desteği

// Add ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
}, ServiceLifetime.Scoped);

// Add Repository Pattern (Specific repositories only)
builder.Services.AddScoped<IMusteriRepository, MusteriRepository>();
builder.Services.AddScoped<IBildirimRepository, BildirimRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Services
builder.Services.AddScoped<IBildirimService, BildirimService>();

// builder.Services.AddDbContext<UserContext>(options =>
// {
//   options.UseSqlite(builder.Configuration.GetConnectionString("UserContext") ?? throw new InvalidOperationException("Connection string 'UserContext' not found."));
// }, ServiceLifetime.Scoped);

var app = builder.Build();

// using (var scope = app.Services.CreateScope())
// {
//   var services = scope.ServiceProvider;

//   SeedData.Initialize(services);
// }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // API Controller routing

app.Run();
