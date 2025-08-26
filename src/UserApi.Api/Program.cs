using System.Text.Json;
using UserApi.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(); // .NET 9 OpenAPI instead of AddSwaggerGen
builder.Services.AddScoped<IUserService, UserService>();

// Configure .NET 9 specific settings
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // .NET 9 OpenAPI mapping
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "UserApi v1");
    });
}

// Add a root endpoint
app.MapGet("/", () => new 
{ 
    message = "Hello from .NET API!",
    timestamp = DateTime.UtcNow,
    version = "1.0.0"
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Make the class public for testing
public partial class Program { }