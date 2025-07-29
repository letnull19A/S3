using Microsoft.EntityFrameworkCore;
using W2B.S3.Contexts;
using W2B.S3.Middlewares;

namespace W2B.S3.RestAPI;

public sealed class WebApiModule(string[] args)
{
    public void Start()
    {
        var builder = WebApplication.CreateBuilder(args);

        // builder.Configuration
        //     .AddJsonFile("appsettings.json")
        //     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
        //     .AddEnvironmentVariables();

        // builder.Services.AddDbContext<S3DbContext>(options =>
        //     options.UseNpgsql(builder.Configuration.GetConnectionString("S3Database")));

        builder.Services.AddControllers();

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

        // if (app.Environment.IsDevelopment())
        // {
        app.UseCors("CORS");
        app.UseSwagger();
        app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}