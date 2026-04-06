
using GestionStock.Application.Abstractions;
using GestionStock.Application.Categories.Commands.Create;
using GestionStock.Application.Categories.Commands.Delete;
using GestionStock.Application.Categories.Commands.Update;
using GestionStock.Application.Categories.Queries.GetAll;
using GestionStock.Application.Categories.Queries.GetById;
using GestionStock.Application.Common.Interfaces;
using GestionStock.Application.Dashboard.Dtos;
using GestionStock.Application.Dashboard.Queries;
using GestionStock.Application.Products.Commands.Create;
using GestionStock.Application.Products.Commands.Delete;
using GestionStock.Application.Products.Commands.Update;
using GestionStock.Application.Products.Queries.GetAll;
using GestionStock.Application.Products.Queries.GetById;
using GestionStock.Application.Stock.Commands.AdjustStock;
using GestionStock.Application.Stock.Commands.DonateStock;
using GestionStock.Application.Stock.Queries.GetBalance;
using GestionStock.Application.Stock.Queries.GetMovements;
using GestionStock.Application.Tenants.Commands.Create;
using GestionStock.Application.Tenants.Commands.Update;
using GestionStock.Application.Tenants.Queries.GetAll;
using GestionStock.Application.Tenants.Queries.GetById;
using GestionStock.Infrastructure.DependencyInjection;
using GestionStock.Infrastructure.Files;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ---------- CORS ----------
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDev", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// ------------------file-----------------------------------
builder.Services.AddScoped<IFileStorageService>(sp =>
{
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    return new LocalFileStorageService(env.WebRootPath);
});

// ---------- AUTH ----------
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var kc = builder.Configuration.GetSection("Keycloak");
        var authority = kc["Authority"] ?? "http://localhost:8080/realms/gestionstock";
        var audience = kc["Audience"] ?? "gestionstock-api";

        options.Authority = authority;

        // IMPORTANT: Keycloak is HTTP in dev
        options.RequireHttpsMetadata = false;

        options.IncludeErrorDetails = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authority,

            ValidateAudience = false,
            // IMPORTANT: aud in your token is ARRAY -> use ValidAudiences
            ValidAudiences = new[] { audience },

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            NameClaimType = "preferred_username",
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                // verify token arrives
                Console.WriteLine("AUTH HEADER => " + ctx.Request.Headers.Authorization.ToString());
                return Task.CompletedTask;
            },

            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("AUTH FAILED => " + ctx.Exception);
                return Task.CompletedTask;
            },

            OnChallenge = ctx =>
            {
                // return reason in response headers (visible in Postman)
                ctx.Response.Headers["x-auth-error"] = ctx.Error ?? "";
                ctx.Response.Headers["x-auth-error-description"] = ctx.ErrorDescription ?? "";
                return Task.CompletedTask;
            },

            OnTokenValidated = ctx =>
            {
                // Map Keycloak realm roles -> ClaimTypes.Role
                var identity = ctx.Principal?.Identity as ClaimsIdentity;
                if (identity is null) return Task.CompletedTask;

                var realmAccess = identity.FindFirst("realm_access")?.Value;
                if (!string.IsNullOrWhiteSpace(realmAccess))
                {
                    using var doc = JsonDocument.Parse(realmAccess);
                    if (doc.RootElement.TryGetProperty("roles", out var roles))
                    {
                        foreach (var r in roles.EnumerateArray())
                        {
                            var role = r.GetString();
                            if (!string.IsNullOrWhiteSpace(role))
                                identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

// ---------- AUTHZ / POLICIES ----------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", p => p.RequireRole("User", "Admin"));
});

// ---------- Controllers ----------
builder.Services.AddControllers();

// ---------- Swagger + JWT ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionStock API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// ---------- Handlers ----------

// Tenants
builder.Services.AddScoped<CreateTenantHandler>();
builder.Services.AddScoped<UpdateTenantHandler>();
builder.Services.AddScoped<GetAllTenantsHandler>();
builder.Services.AddScoped<GetTenantByIdHandler>();

// Categories
builder.Services.AddScoped<CreateCategoryHandler>();
builder.Services.AddScoped<UpdateCategoryHandler>();
builder.Services.AddScoped<DeleteCategoryHandler>();
builder.Services.AddScoped<GetCategoriesHandler>();
builder.Services.AddScoped<GetCategoryByIdHandler>();

// Products
builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<UpdateProductHandler>();
builder.Services.AddScoped<DeleteProductHandler>();
builder.Services.AddScoped<GetProductsHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();

// Stock
builder.Services.AddScoped<AdjustStockHandler>();
builder.Services.AddScoped<DonateStockHandler>();
builder.Services.AddScoped<GetBalanceHandler>();
builder.Services.AddScoped<GetMovementsHandler>();

builder.Services.AddScoped<IQueryHandler<GetDashboardQuery, DashboardDto>, GetDashboardHandler>();






// ---------- Infrastructure ----------
//builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// ---------- Pipeline ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("ReactDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();