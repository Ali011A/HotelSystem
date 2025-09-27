using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        public bool SaveChanges();
        public void Dispose();
        Task<bool> SaveChangesAsync();
    }
}
