using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Controllers;
using BazarCarioca.WebAPI.DTOs.Mapper;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Para funcionar fora do servidor local
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Add services to the container.
#region services

//JSON necessários para POST e PATCH
builder.Services
    .AddControllers()
    .AddNewtonsoftJson()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });

/* caso vá mudar o banco de dados, adicione mais uma string como a de baixo
* (lembrando de mudar também em:
* appsettings.json > ConnectionStrings) */
string MySqlConnection = builder.Configuration.GetConnectionString("AWSRDS");

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(MySqlConnection,
        ServerVersion.AutoDetect(MySqlConnection))
);

// Registrando o Web Service
builder.Services.AddScoped<IWebService, S3Service>();

// Registrando os Repositories
builder.Services.AddScoped<IShopkeeperRepository, ShopkeeperRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Para o SQLController
builder.Services.AddScoped<ProductsController>();
builder.Services.AddScoped<StoresController>();

// Para a autênticação JWT
builder.Services.AddScoped<ITokenService, TokenService>();

// Automapper para conversão entre DTOs e Entidades
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Autorização/autenticação
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddIdentity<ApplicationUser , IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var secretKey = builder.Configuration["JWT:SecretKey"]
    ?? throw new ArgumentException("Invalid secret key.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});


#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bazar Carioca");
    });
}

// Configure the HTTP request pipeline.

// Linha comentada pois HTTPS não está configurado no servidor
//app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
