using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var username = builder.Configuration["Database:Username"];
var password = builder.Configuration["Database:Password"];
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
  ?.Replace("{Database:Username}", username)
  .Replace("{Database:Password}", password);

builder.Services.AddDbContext<DataContext>(options =>
{
  options.UseNpgsql(connectionString); // Pass the constructed connection string
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();