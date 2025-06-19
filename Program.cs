using Microsoft.EntityFrameworkCore;
using WarApi.Models;
using WarApi.Repositories;
using WarApi.Repositories.Interfaces;
using MatchReportNamespace.Repositories;
using MatchReportNamespace.Services;

var builder = WebApplication.CreateBuilder(args);

// Convertir DATABASE_URL a connection string compatible con Npgsql
string ConvertDatabaseUrlToConnectionString(string? databaseUrl)
{
    if (string.IsNullOrWhiteSpace(databaseUrl))
        throw new ArgumentException("DATABASE_URL is null or empty.");

    if (!databaseUrl.StartsWith("postgres://"))
        throw new ArgumentException("DATABASE_URL must be a valid PostgreSQL URI.");

    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    if (userInfo.Length != 2)
        throw new ArgumentException("DATABASE_URL user info is invalid.");

    if (uri.Port <= 0)
        throw new ArgumentException("DATABASE_URL must include a valid port.");

    return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};Ssl Mode=Require;Trust Server Certificate=true";
}

// Obtener y convertir la cadena
var rawUrl = builder.Configuration["DATABASE_URL"];
var connectionString = ConvertDatabaseUrlToConnectionString(rawUrl);

// Registrar DbContext con la cadena procesada
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Inyección de dependencias
builder.Services.AddScoped<IMatchReportRepository, MatchReportRepository>();
builder.Services.AddScoped<IMatchReportService, MatchReportService>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Aplicar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configuración HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
