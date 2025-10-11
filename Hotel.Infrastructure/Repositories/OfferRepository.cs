using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Models;
using Hotel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext _context;

        public OfferRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Offer offer)
        {
            await _context.Offers.AddAsync(offer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Offer offer)
        {
            _context.Offers.Update(offer);
            await _context.SaveChangesAsync();
        }

        public async Task<Offer?> GetByIdAsync(Guid id)
        {
            return await _context.Offers
                .Where(o => o.Id == id && !o.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Offer>> GetAllAsync()
        {
            return await _context.Offers
                .Where(o => !o.IsDeleted)
                .ToListAsync();

        }

        public async Task SoftDeleteAsync(Offer offer)
        {
            offer.IsDeleted = true;
            _context.Offers.Update(offer);
            await _context.SaveChangesAsync();
        }
    }

}
