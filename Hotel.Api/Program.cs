
using Hotel.Application.Services;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotel.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


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

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
          //  app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
