using MatchReportNamespace.Repositories;
using MatchReportNamespace.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using WarApi.Models;
using WarApi.Repositories;
using WarApi.Repositories.Interfaces;
using WarApi.Services;
using WarApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WarApi.Services.Security;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores con manejo para referencias cíclicas
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// Agregar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT token"
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
            Array.Empty<string>()
        }
    });
});

// Configurar CORS para permitir cualquier origen, encabezado y método
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Inyección de dependencias
builder.Services.AddScoped<IMatchReportRepository, MatchReportRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IMatchReportService, MatchReportService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IPlayerStatsService, PlayerStatsService>();
builder.Services.AddScoped<IArmyListRepository, ArmyListRepository>();
builder.Services.AddScoped<IArmyListService, ArmyListService>();
builder.Services.AddSingleton<IEncryptionService, Base64EncryptionService>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Identity.PasswordHasher<WarApi.Models.Player>>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? builder.Configuration.GetConnectionString("DATABASE_URL")
                      ?? builder.Configuration["DATABASE_URL"]
                      ?? Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Database connection string is not configured.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var keyBytes = Encoding.ASCII.GetBytes(builder.Configuration["JWT_SECRET"] ?? "dev-secret-key");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });





var app = builder.Build();

// Middleware de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var pendingMigrations = db.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
    {
        Console.WriteLine("⚠️  Applying pending EF Core migrations...");
        db.Database.Migrate();
        Console.WriteLine("✅ Migrations applied successfully.");
    }
    else
    {
        Console.WriteLine("ℹ️  No pending migrations. Database is up to date.");
    }

    var hasher = scope.ServiceProvider.GetRequiredService<PasswordHasher<Player>>();
    var playersToUpdate = db.Players.Where(p => !p.Contraseña.StartsWith("AQAAAA", StringComparison.Ordinal)).ToList();
    if (playersToUpdate.Any())
    {
        Console.WriteLine($"⚠️  Found {playersToUpdate.Count} unhashed password(s). Hashing...");
        foreach (var p in playersToUpdate)
        {
            p.Contraseña = hasher.HashPassword(p, p.Contraseña);
        }
        db.SaveChanges();
        Console.WriteLine("✅ Passwords hashed successfully.");
    }
    else
    {
        Console.WriteLine("ℹ️  All passwords already hashed.");
    }
}
catch (Exception ex)
{
    Console.WriteLine("❌ Error applying migrations:");
    Console.WriteLine(ex.ToString());
}



app.UseHttpsRedirection();

// Habilitar la política de CORS definida anteriormente
app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();



