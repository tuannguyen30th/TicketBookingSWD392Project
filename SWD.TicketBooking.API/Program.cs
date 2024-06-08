using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using SWD.TicketBooking.API.Installer;
using SWD.TicketBooking.API.Middlewares;
using SWD.TicketBooking.Repo.Entities;
using SWD.TicketBooking.Repo.SeedData;
using SWD.TicketBooking.API.Installer;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using SWD.TicketBooking.Service.Exceptions;


namespace SWD.TicketBooking.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.InstallerServicesInAssembly(builder.Configuration);

            /*
              builder.Services.AddEndpointsApiExplorer();
               builder.Services.AddSwaggerGen(option =>
               {
                   option.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketBooking API", Version = "v1" });
                   option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                       In = ParameterLocation.Header,
                       Description = "Please enter a valid token",
                       Name = "Authorization",
                       Type = SecuritySchemeType.Http,
                       BearerFormat = "JWT",
                       Scheme = "Bearer"
                   });
                   option.AddSecurityRequirement(new OpenApiSecurityRequirement
       {
           {
               new OpenApiSecurityScheme
               {
                   Reference = new OpenApiReference
                   {
                       Type=ReferenceType.SecurityScheme,
                       Id="Bearer"
                   }
               },
               new string[]{}
           }
       });
               });*/


            /*  builder.Services.AddCors(option =>
                  option.AddPolicy("CORS", builder =>
                      builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((host) => true)));*/

            var app = builder.Build();

            // Hook into application lifetime events and trigger only application fully started 
            app.Lifetime.ApplicationStarted.Register(async () =>
            {
                // Database Initialiser 
                await app.InitialiseDatabaseAsync();
            });
            // Configure the HTTP request pipeline.
            /* if (app.Environment.IsDevelopment())
             {
                 await using (var scope = app.Services.CreateAsyncScope())
                 {
                     var dbContext = scope.ServiceProvider.GetRequiredService<TicketBookingDbContext>();
                     await dbContext.Database.MigrateAsync();
                 }

                 app.UseSwagger();
                 app.UseSwaggerUI();

             }
             else
             {*/

            app.UseSwagger(op => op.SerializeAsV2 = false);
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            /*    Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .WriteTo.Console()
              .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
              .CreateLogger();

                Log.Information("Hello, world!");

                int a = 10, b = 0;
                try
                {
                    Log.Debug("Dividing {A} by {B}", a, b);
                    Console.WriteLine(a / b);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Something went wrong");
                }
                finally
                {
                    await Log.CloseAndFlushAsync();
                }*/

            ApplyMigration(app);

            app.UseCors("CORS");

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            await app.RunAsync();
        }
        private static void ApplyMigration(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<TicketBookingDbContext>();
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
        }
    }
}