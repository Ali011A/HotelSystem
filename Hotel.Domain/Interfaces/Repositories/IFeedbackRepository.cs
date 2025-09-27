using Hotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces.Repositories
{
    public interface IFeedbackRepository
    {
        Task AddAsync(Feedback feedback);
        Task<Feedback> GetByIdAsync(Guid id, CancellationToken cancellation = default);
       
        Task UpdateAsync(Feedback feedback);
        Task SoftDeleteAsync(Guid id, CancellationToken cancellation = default);
        Task<bool> CustomerHasCompletedReservationAsync(Guid reservationId);
        Task<List<Feedback>> GetAllFeedBaksAsync();
    }
}
