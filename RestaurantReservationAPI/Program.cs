using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Data;
using RestaurantReservationAPI.Helpers;
using RestaurantReservationAPI.Interface;
using RestaurantReservationAPI.Middleware;
using RestaurantReservationAPI.Repository;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var isProduction = builder.Environment.IsProduction();

// ✅ Load `.env` only in Production
var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");

if (File.Exists(envFilePath))
{
    Env.Load(envFilePath);
    Console.WriteLine("✅ Loaded .env file successfully.");
}
else
{
    Console.WriteLine("⚠️ Warning: `.env` file not found. Ensure environment variables are set.");
}

// ✅ Load configuration (appsettings.json + Environment)
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// ✅ Retrieve database credentials (Fallbacks: `.env` → Environment Variables)
var dbUser = Env.GetString("DB_USER") ?? Environment.GetEnvironmentVariable("DB_USER") ?? throw new InvalidOperationException("DB_USER not set.");
var dbPassword = Env.GetString("DB_PASSWORD") ?? Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("DB_PASSWORD not set.");

// ✅ Retrieve API port (Defaults: `5101` for Dev, `3020` for Prod)
var port = isProduction ? (Env.GetString("API_PORT") ?? "3020") : "5101";

// ✅ Retrieve and configure database connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?.Replace("{DB_USER}", dbUser)
    ?.Replace("{DB_PASSWORD}", dbPassword);

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("❌ The connection string is not configured. Check `appsettings.json` and `.env`.");
}

Console.WriteLine($"🔗 Using Database User: {dbUser} | Connecting to DB...");

// ✅ Apply Production settings (Force API to listen on correct port)
if (isProduction)
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
    Console.WriteLine($"🚀 Running in PRODUCTION mode on port {port}");
}
else
{
    Console.WriteLine($"🛠 Running in DEVELOPMENT mode on port {port}");
}

// ✅ Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddAutoMapper(typeof(MappingProfiles));

// ✅ Register repositories
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IOpeningHoursRepository, OpeningHoursRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Configure database with the correctly formatted connection string
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString));

Console.WriteLine("✅ Database connection initialized.");

// ✅ Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration["Frontend:Url"] ?? Env.GetString("FRONTEND_URL") ?? "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// ✅ Apply database migrations in Production
if (isProduction)
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            dbContext.Database.Migrate();
            Console.WriteLine("✅ Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Database migration failed: {ex.Message}");
            throw;
        }
    }
}

// ✅ Configure Swagger globally
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantReservationAPI v1");
});

// ✅ Middleware Order
app.UseCors("AllowFrontend");
app.UseErrorHandling(); // Falls Middleware für Fehlerhandling existiert
app.UseAuthorization();
app.MapControllers();

app.Run();