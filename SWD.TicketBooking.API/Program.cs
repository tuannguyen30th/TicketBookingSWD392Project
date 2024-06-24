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
using Microsoft.AspNetCore.Mvc;


namespace SWD.TicketBooking.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.InstallerServicesInAssembly(builder.Configuration);
            builder.Services.AddResponseCaching();

            var app = builder.Build();

            //app.Lifetime.ApplicationStarted.Register(async () =>
            //{
            //    await app.InitialiseDatabaseAsync();
            //});      
            app.UseSwagger(op => op.SerializeAsV2 = false);
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            ApplyMigration(app);

            app.UseCors("CORS");

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();
            app.UseResponseCaching();
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