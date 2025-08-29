using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Controllers;
using BazarCarioca.WebAPI.DTOs.Mapper;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using BazarCarioca.WebAPI.Services;
using BazarCarioca.WebAPI.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// cors aqui?
var OriginWithAuthorizedAcess = "_originWithAuthorizedAcess";

builder.Services.AddCors(options =>
    options.AddPolicy(name: OriginWithAuthorizedAcess, policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://bazar-carioca-frontend.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    })
);

//fim do cors

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
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    )
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

// Para a autênticação JWT
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserValidate, UserValidate>();

// Automapper para conversão entre DTOs e Entidades
builder.Services.AddAutoMapper(typeof(ProductMappingProfile));

builder.Services.AddEndpointsApiExplorer();

//Autorização/autenticação
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT ",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
   });
});

builder.Services.AddIdentity<ApplicationUser , IdentityRole>(options =>
{
    // Senha
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // Permitir espaço no username (ex: "JOAO DA SILVA")
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
})
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy
    .RequireRole("Admin"));

    options.AddPolicy("SuperAdminOnly", policy => policy
        .RequireRole("Admin")
        .RequireClaim("id", "Natan"));

    options.AddPolicy("ExclusivePolicyOnly", policy =>
    {
        policy.RequireAssertion(context => 
            context.User.HasClaim(Claim => Claim.Type == "id" && Claim.Value=="Natan")
            || context.User.IsInRole("SuperAdmin"));
    });
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

app.Use(async (context, next) =>
{
    var origin = context.Request.Headers["Origin"].FirstOrDefault();
    Console.WriteLine($"Origin header: {origin ?? "<none>"}");
    await next();
});

app.UseCors(OriginWithAuthorizedAcess);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
