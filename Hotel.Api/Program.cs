
using Hotel.Api.Middleware;
using Hotel.Application.AutoMapper.Mappings;
using Hotel.Application.Interfaces.Queries;
using Hotel.Application.Services;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Hotel.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            // ---------- Logging ----------
            

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            // Register DbContext FIRST
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sql =>
                {
                    sql.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
                });

                options.LogTo(msg => Debug.WriteLine(msg), LogLevel.Information);

                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }

          
                // options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IReservationReadRepository, ReservationRepository>();
            // builder.Services.AddScoped<GlobalErrorHandlingMiddleware>();
            builder.Services.AddScoped<TransactionMiddleware>();
            var app = builder.Build();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            app.UseMiddleware<TransactionMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
