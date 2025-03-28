using BookClub2._0.Interfaces;
using BookClub2._0.Repositories;
using BookClub2._0;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// CORS-konfiguration (tillader alle anmodninger, kan strammes op senere)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetPreflightMaxAge(TimeSpan.FromSeconds(14440));
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ?? Sørg for, at din connection string findes i `appsettings.json`
builder.Services.AddDbContext<BookClubDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ?? Registrér repository (bruger `IRepository` interfacet)
builder.Services.AddScoped<IRepository, UserRepository>();

var app = builder.Build();

// ?? Aktivér Swagger, hvis du kører i udviklingsmiljø
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ?? Middleware-konfiguration
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ?? Start applikationen
app.Run();
