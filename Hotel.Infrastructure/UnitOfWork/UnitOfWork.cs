using Hotel.Domain.Interfaces.UnitOfWork;
using Hotel.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.UnitOfWork
{
   public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext ApplicationDbContext { get; set; }

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        public bool SaveChanges()
        {
            try
            {
                ApplicationDbContext.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await ApplicationDbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            ApplicationDbContext.Dispose();
        }
    }
}
