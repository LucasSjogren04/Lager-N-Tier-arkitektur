using Lager_N_Tier_arkitektur.Middleware;
using Lager_N_Tier_arkitektur.Repositories;
using Lager_N_Tier_arkitektur.Services;
using Microsoft.EntityFrameworkCore;

namespace Lager_N_Tier_arkitektur
{
    // =================================================================
    //  LAGER-/N-TIER-ARKITEKTUR
    //
    //   Presentation  Controllers/, Middleware/      HTTP in/out
    //        |  IStudentService + DTOs
    //        v
    //   Business      Services/, Dtos/               rules + mapping
    //        |  IStudentRepository + Students entity
    //        v
    //   Data          Repositories/, Data/, Models/, Migrations/
    //        |  EF Core
    //        v
    //   SQL Server
    //
    //  Rule: each layer only calls the layer directly below it.
    // =================================================================
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("Lager_N_Tier_arkitekturContext") ?? throw new InvalidOperationException("Connection string 'Lager_N_Tier_arkitekturContext' not found.");

            // ---- Data layer ----
            builder.Services.AddDbContext<Lager_N_Tier_arkitekturContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            // ---- Business layer ----
            builder.Services.AddScoped<IStudentService, StudentService>();

            // ---- Presentation layer ----
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // ---- Middleware pipeline (order matters) ----
            // First in = last out: catches exceptions from everything below it.
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
