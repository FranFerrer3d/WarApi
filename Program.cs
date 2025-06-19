using MatchReportNamespace.Repositories;
using MatchReportNamespace.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
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
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMatchReportRepository, MatchReportRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IMatchReportService, MatchReportService>(); 
builder.Services.AddScoped<IPlayerService, PlayerService>();



var rawUrl = builder.Configuration["ConnectionStrings:DefaultConnection"] ?? 
             builder.Configuration["ConnectionStrings:DATABASE_URL"];

var connectionString = rawUrl;

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
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
}
catch (Exception e) { 
    Console.WriteLine(e.Message);
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



