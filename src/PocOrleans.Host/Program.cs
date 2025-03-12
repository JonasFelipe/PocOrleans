using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using Orleans.Hosting;
using PocOrleans.Infrastructure.Data;
using PocOrleans.Infrastructure;
using PocOrleans.Infrastructure.Repositories;
using PocOrleans.Infrastructure.Data;
using PocOrleans.Infrastructure.Services;
using PocOrleans.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=localhost;Database=PocOrleans;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"));

// Configure Orleans
builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("Default");
});

// Register services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.Run();