using Microsoft.EntityFrameworkCore;
using FlightDelay.Api.Persistence;
using FlightDelay.Api.Middleware;
using FluentValidation;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure database
builder.Services.AddDbContext<FlightDelayDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Data Source=flightdelay.db"));

// Register repositories and services
builder.Services.AddScoped<IFlightRepository, EfCoreFlightRepository>();
builder.Services.AddScoped<FlightDataImporter>();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add memory cache for idempotency
builder.Services.AddMemoryCache();

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Add controllers
builder.Services.AddControllers();

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<FlightDelayDbContext>();

// Add exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Add exception handling
app.UseExceptionHandler();

// Add correlation ID middleware
app.UseMiddleware<CorrelationIdMiddleware>();

// Add API key authentication middleware
app.UseMiddleware<ApiKeyAuthMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

// Health check endpoints
app.MapHealthChecks("/health/live");
app.MapHealthChecks("/health/ready");

// Flight API endpoints
app.MapGet("/api/flights", async (IFlightRepository repo, CancellationToken ct) =>
{
    var flights = await repo.GetDelayedFlightsAsync(0, ct);
    return Results.Ok(flights.Take(100)); // Return first 100
})
.WithName("GetFlights")
.WithOpenApi();

app.MapPost("/api/import", async (FlightDataImporter importer, ILogger<Program> logger) =>
{
    var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data", "flights.csv");
    logger.LogInformation("Importing from {Path}", csvPath);
    
    if (!File.Exists(csvPath))
    {
        return Results.NotFound($"CSV file not found at {csvPath}");
    }
    
    var result = await importer.ImportFromCsvAsync(csvPath);
    return Results.Ok(result);
})
.WithName("ImportFlightData")
.WithOpenApi();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}