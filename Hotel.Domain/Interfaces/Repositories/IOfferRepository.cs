using Hotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces.Repositories
{
    public interface IOfferRepository
    {
        Task AddAsync(Offer offer);
        Task UpdateAsync(Offer offer);
        Task<Offer?> GetByIdAsync(Guid id);
        Task<IEnumerable<Offer>> GetAllAsync();
        Task SoftDeleteAsync(Offer offer);
    }
}
