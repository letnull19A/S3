using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using W2B.S3.Contexts;
using W2B.S3.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<S3DbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("S3Database")));

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", p =>
    {
        p.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseNonEmptyFiles();

if (app.Environment.IsDevelopment())
{
    app.UseCors("CORS");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();