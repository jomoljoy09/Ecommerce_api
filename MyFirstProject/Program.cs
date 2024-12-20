using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();  // For ASP.NET Core 6+
builder.Services.AddSwaggerGen();

// Configure CORS (Allow requests from your React app running on localhost:3000)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
        builder.WithOrigins("http://localhost:3000", "https://localhost:44353", "http://localhost:3002", "http://localhost:3001")  // React app's URL
               .AllowAnyHeader()
               .AllowAnyMethod());
});

// Configure Kestrel to allow larger file uploads (if necessary)
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 104857600;  // 100 MB (adjust as needed)
});

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104857600;  // 100 MB (adjust as needed)
});

var app = builder.Build();

// Enable Swagger UI if the environment is development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS for your API
app.UseCors("AllowReactApp");

// Enable other middleware (such as static files, authentication, etc.)
app.UseRouting();

// Map controllers (API endpoints)
app.MapControllers();

// Run the application
app.Run();
