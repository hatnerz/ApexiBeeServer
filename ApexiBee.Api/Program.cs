using ApexiBee.Persistance.Database;
using ApexiBee.Persistance.EntityConfiguration;
using ApexiBee.Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ApexiBee.Application.Interfaces;
using ApexiBee.Infrastructure.Implementation.Services;
using ApexiBee.Infrastructure.Implementation.Repositories;
using ApexiBee.Infrastructure.Interfaces;
using ApexiBee.Application.Helpers;
using ApexiBee.Application.DomainServices;
using ApexiBee.API.Middleware;
using Microsoft.SqlServer.Management.Smo.Wmi;
using AutoMapper;
using ApexiBee.Infrastructure.Implementation.AutoMapper;
using ApexiBee.Infrastructure.Implementation.Helpers;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BeeDbContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString("azure"), b => b.MigrationsAssembly("ApexiBee.Persistance"));
});

// Configuring identity using ASP.NET Core Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 0;
    })
    .AddEntityFrameworkStores<BeeDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container (services dependency injection).
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IApiaryService, ApiaryService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddSingleton<ConfigurationHelper>();

builder.Services.AddScoped<JwtHelper>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.Run();
