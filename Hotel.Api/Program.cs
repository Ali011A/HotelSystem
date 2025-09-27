
using Hotel.Application.AutoMapper;
using Hotel.Application.Service;
using Hotel.Application.Services;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Interfaces.UnitOfWork;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Repositories;
using Hotel.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

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
