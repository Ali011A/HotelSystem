
using Hotel.Api.AuoroMapper;
using Hotel.Api.Middleware;
using Hotel.Application.Interfaces.Queries;
using Hotel.Application.InterfaceServices;
using Hotel.Application.Services;
//using IUnitOfWork = Hotel.Domain.Interfaces.Repositories.IUnitOfWork;
//using UnitOfWork = Hotel.Infrastructure.Repositories.UnitOfWork;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Repositories;
using Hotel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            // Replace this line:
            builder.Services.AddAutoMapper(cfg => { }, typeof(AuotoMaperProfile).Assembly);

            // With this line:
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

            builder.Services.AddDbContext<ApplicationDbContext>(optionsAction => optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("Local")));


            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(AuotoMaperProfile).Assembly);
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IReservationReadRepository, ReservationRepository>();
            // builder.Services.AddScoped<GlobalErrorHandlingMiddleware>();
            builder.Services.AddScoped<TransactionMiddleware>();
            builder.Services.AddScoped<IReservationReadRepository, ReservationRepository>();
            // builder.Services.AddScoped<GlobalErrorHandlingMiddleware>();
            builder.Services.AddScoped<TransactionMiddleware>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
