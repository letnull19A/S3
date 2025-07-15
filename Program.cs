using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Web.DependencyInjection;
using W2B.S3.Contexts;
using W2B.S3.Interfaces;
using W2B.S3.Middleware;
using W2B.S3.Models;
using W2B.S3.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<S3DbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("S3Database")));

builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddSingleton<ImageProcessor>();
builder.Services.AddImageSharp();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<S3DbContext>();
    var apiKeyService = scope.ServiceProvider.GetRequiredService<IApiKeyService>();

    await InitializeRootUser(dbContext, apiKeyService);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
return;

async Task InitializeRootUser(S3DbContext db, IApiKeyService keyService)
{
    if (!await db.ApiKeys.AnyAsync())
    {
        var rootKey = await keyService.CreateKeyAsync(
            owner: "root",
            permissions: "read,write,delete,manage,admin",
            expiresAt: null);

        Console.WriteLine("====================================");
        Console.WriteLine("ROOT ACCESS KEY CREATED");
        Console.WriteLine($"Key: {rootKey.Id}");
        Console.WriteLine($"Owner: {rootKey.Owner}");
        Console.WriteLine($"Permissions: {rootKey.Permissions}");
        Console.WriteLine("====================================");
        Console.WriteLine("WARNING: Store this key securely!");
        Console.WriteLine("====================================");
    }
    else
    {
        Console.WriteLine("System already initialized - skipping root key creation");
    }
}