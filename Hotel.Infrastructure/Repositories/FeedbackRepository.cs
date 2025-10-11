using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Models;
using Hotel.Infrastructure.Persistence;
using Hotel.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public async Task<int>  AddAsync(Feedback feedback)
        {
            await  Context.Feedbacks.AddAsync(feedback);
            
            return await Context.SaveChangesAsync();    

         
        }
        public async Task<Feedback> GetByIdAsync(Guid id, CancellationToken cancellation = default)
        {
            return await Context.Feedbacks
                .Include(f => f.Reservation)
                .FirstOrDefaultAsync(f => f.Id == id, cancellation);
        }

        public async Task<int> UpdateAsync(Feedback feedback)
        {
            Context.Feedbacks.Update(feedback);
         return await  Context.SaveChangesAsync();
           
        }
        public async Task<int> SoftDeleteAsync(Guid id, CancellationToken cancellation = default)
        {
            var entity = await GetByIdAsync(id, cancellation);

            if (entity == null)
            {
               
                return 0;
            }

           
            entity.IsDeleted = true;
            Context.Update(entity); 
            
            return await Context.SaveChangesAsync(cancellation);

        }
        public async Task<bool> CustomerHasCompletedReservationAsync(Guid reservationId)
        {
            var isCompleted = await Context.Reservations
                .Where(r => r.Id == reservationId && r.Status == ReservationStatus.Completed)
                .AnyAsync();

            return isCompleted;
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
    