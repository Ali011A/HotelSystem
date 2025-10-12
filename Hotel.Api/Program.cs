
using Hotel.Api.Middleware;

using Hotel.Application.Interfaces.Queries;
using Hotel.Application.Services;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Hotel.Application.Service;
using Hotel.Application.Services;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Interfaces.UnitOfWork;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Repositories;
using Hotel.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IUnitOfWork = Hotel.Domain.Interfaces.Repositories.IUnitOfWork;
using UnitOfWork = Hotel.Infrastructure.Repositories.UnitOfWork;
using Hotel.Domain.Interfaces;

namespace Hotel.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            // ---------- Logging ----------
            

            var connectionString = builder.Configuration.GetConnectionString("Local");
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


            // Replace this line:
            builder.Services.AddAutoMapper(cfg => { }, typeof(FeedbackProfile).Assembly);

            // With this line:
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

            builder.Services.AddDbContext<ApplicationDbContext>(optionsAction => optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("Local")));


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


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {

                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")) 
                       .LogTo(log => Debug.WriteLine(log) , LogLevel.Information);        
                
            });


            builder.Services.AddScoped<IOfferRepository, OfferRepository>();

            builder.Services.AddScoped<IOfferService, OfferService>();


            // تسجيل HttpContextAccessor لو هتحتاج StaffId من Claims
          //  builder.Services.AddHttpContextAccessor();



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
            app.UseAuthentication();

            app.UseAuthentication();
          //  app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
