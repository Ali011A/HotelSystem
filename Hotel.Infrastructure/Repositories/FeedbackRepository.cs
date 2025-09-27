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
    public class FeedbackRepository : IFeedbackRepository
    {
       ApplicationDbContext   Context {  get; set; }

        public FeedbackRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public async Task  AddAsync(Feedback feedback)
        {
            await  Context.Feedbacks.AddAsync(feedback);
         
        }
        public async Task<Feedback> GetByIdAsync(Guid id, CancellationToken cancellation = default)
        {
            return await Context.Feedbacks
                .Include(f => f.Reservation)
                .FirstOrDefaultAsync(f => f.Id == id, cancellation);
        }

        public async Task UpdateAsync(Feedback feedback)
        {
            Context.Feedbacks.Update(feedback);
           
        }
        public async Task SoftDeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var f = await Context.Feedbacks.FindAsync(new object[] { id }, cancellation);
            if (f == null) return;
            f.IsDeleted = true;
            
        }
        public async Task<bool> CustomerHasCompletedReservationAsync(Guid reservationId)
        {
            var res = await Context.Reservations
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (res == null)
                return false;

                return
                res.CheckoutDate <= DateTime.UtcNow;
        }

        public async Task<List<Feedback>> GetAllFeedBaksAsync()
        {
            return await Context.Feedbacks
                 .Where(f => !f.IsDeleted) 
                   .OrderBy(f => f.CreatedAt) 
                     .ToListAsync();
        }

    }
}
    