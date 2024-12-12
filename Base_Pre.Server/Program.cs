

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using Base_Pre.Server.Models;

namespace Base_Pre.Server
{
    public class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LoadLibrary(string lpFileName);

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Configuration.AddConfiguration(configuration);

            // Configure Kestrel to use port 5001
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenLocalhost(5001);
            });

            // Add HTTP logging
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.All;
                logging.RequestHeaders.Add("Origin");
                logging.ResponseHeaders.Add("Access-Control-Allow-Origin");
            });

            // Add memory cache for server-side caching
            builder.Services.AddMemoryCache();

            // Register the database context with explicit connection string
            builder.Services.AddDbContext<PrimaryDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));

            // Configure CORS policy for Vite client
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:5001") // Updated to match new port
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .WithExposedHeaders("Content-Disposition")
                           .SetIsOriginAllowed(origin => true);
                });
            });

            // Add other services
            builder.Services.AddHttpClient();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
           
          

            // Configure Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Base_Pre API",
                    Version = "v1.012",
                    Description = "API for Base_Pre Application"
                });

                // Update server configuration for new port
                c.AddServer(new OpenApiServer
                {
                    Url = "http://localhost:5000",
                    Description = "Development Server"
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Configure Swagger UI
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentname}/swagger.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Base_Pre API V1");
                    c.RoutePrefix = "swagger";
                });
            }

            // Important: Order matters for middleware
            app.UseHttpLogging();

            // CORS middleware must be before routing and endpoints
            app.UseCors();

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();

            // Print startup message with correct port
            Console.WriteLine($"Application starting on port 5001");

            app.Run();
        }
    }
}








