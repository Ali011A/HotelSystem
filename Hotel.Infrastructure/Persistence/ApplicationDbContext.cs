using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Persistence
{
    public class ApplicationDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-4H6V1FJ;Database=HotelDb;Trusted_Connection=True;MultipleActiveResultSets=true");   
        }
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    }
}
