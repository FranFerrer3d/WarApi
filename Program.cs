using MatchReportNamespace.Repositories;
using MatchReportNamespace.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Swashbuckle.AspNetCore.SwaggerUI;
using WarApi.Models;
using WarApi.Repositories;
using WarApi.Repositories.Interfaces;
using WarApi.Services;
using WarApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Agregar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias
builder.Services.AddScoped<IMatchReportRepository, MatchReportRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IMatchReportService, MatchReportService>(); 
builder.Services.AddScoped<IPlayerService, PlayerService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? builder.Configuration.GetConnectionString("DATABASE_URL")
                      ?? builder.Configuration["DATABASE_URL"]
                      ?? Environment.GetEnvironmentVariable("DATABASE_URL");

connectionString = ConvertUrlIfNeeded(connectionString);

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Database connection string is not configured.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));





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
}
catch (Exception ex)
{
    Console.WriteLine("❌ Error applying migrations:");
    Console.WriteLine(ex.ToString());
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static string ConvertUrlIfNeeded(string? conn)
{
    if (string.IsNullOrWhiteSpace(conn))
    {
        return conn ?? string.Empty;
    }

    if (!conn.Contains("://"))
    {
        return conn;
    }

    var uri = new Uri(conn);
    var userInfo = uri.UserInfo.Split(':', 2);
    var builder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = userInfo.Length > 0 ? userInfo[0] : string.Empty,
        Password = userInfo.Length > 1 ? userInfo[1] : string.Empty,
        Database = uri.AbsolutePath.TrimStart('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    };

    return builder.ToString();
}



