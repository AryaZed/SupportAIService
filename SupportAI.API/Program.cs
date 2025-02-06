using Carter;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SupportAI.Application.Interfaces.Repo;
using SupportAI.Application.Interfaces.Services;
using SupportAI.Application.Services;
using SupportAI.Domain.Entities;
using SupportAI.Domain.Entities.Identity;
using SupportAI.Infrastructure.Configurations;
using SupportAI.Infrastructure.Persistence;
using SupportAI.Infrastructure.Repositories;
using SupportAI.ML.Services;
using SupportAI.Shared.Configuration;
using SupportAI.Shared.Hubs;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Bind Configuration Using Options Pattern
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSignalR();

// Configure Database Using Options Pattern
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var dbSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    options.UseSqlServer(dbSettings.ConnectionString);
});

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

// Register MediatR (CQRS)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Register Carter for Minimal APIs
builder.Services.AddCarter();

// Register Services & Repositories
builder.Services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
builder.Services.AddScoped<ISupportTicketService, SupportTicketService>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddSingleton<TicketAIService>(); 

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();

app.MapHub<TicketHub>("/ticketHub");

app.Run();
