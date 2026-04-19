using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Multi_Tenant_E_Commerce_API.Data;
using Multi_Tenant_E_Commerce_API.Services.Authorization;
using Multi_Tenant_E_Commerce_API.Services.CompanyService;
using Multi_Tenant_E_Commerce_API.Services.CustomerService;
using Multi_Tenant_E_Commerce_API.Services.FinanceService;
using Multi_Tenant_E_Commerce_API.Services.ItemsService;
using Multi_Tenant_E_Commerce_API.Services.UserService;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Use built-in OpenAPI (already installed)
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IItemsService, ItemsService>();
builder.Services.AddScoped<ICustomerCartService, CustomerCartService>();
builder.Services.AddScoped<IFinanceService, FinanceService>();
builder.Services.AddScoped<ITenantAuthorizationService, TenantAuthorizationService>();

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is missing!");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();