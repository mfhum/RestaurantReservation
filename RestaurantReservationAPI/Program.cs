using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Helpers;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Middleware;
using RestaurantReservationAPI.Repository;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
  });

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IOpeningHoursRepository, OpeningHoursRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database
var username = builder.Configuration["Database:Username"] ?? "defaultuser";
var password = builder.Configuration["Database:Password"] ?? "defaultpassword";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
  ?.Replace("{Database:Username}", username)
  ?.Replace("{Database:Password}", password);

if (string.IsNullOrEmpty(connectionString))
{
  throw new InvalidOperationException("The connection string is not configured.");
}

builder.Services.AddDbContext<DataContext>(options =>
{
  options.UseNpgsql(connectionString); // PostgreSQL
});

// Configure CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowFrontend", policy =>
  {
    policy.WithOrigins("http://localhost:5173") // Dein React-Frontend
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials(); // Falls Auth oder Cookies genutzt werden
  });
});

var app = builder.Build();

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
  var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
  dbContext.Database.Migrate();
}

// Configure Swagger globally
app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantReservationAPI v1");
});

// Middleware-Reihenfolge optimieren
app.UseCors("AllowFrontend");
app.UseErrorHandling(); // Falls Middleware für Fehlerhandling existiert
app.UseAuthorization();
app.MapControllers();

app.Run();