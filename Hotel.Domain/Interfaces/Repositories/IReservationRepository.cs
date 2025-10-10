using Hotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces.Repositories
{
    public interface IReservationRepository: IGenericRepository<Reservation>
    {


        //علشان نحدد إذا كان الغرفة متاحة للحجز
        //Task<bool> IsRoomAvailableAsync(Guid roomId, DateOnly checkin, DateOnly checkout,
        // Guid? excludeReservationId = null, CancellationToken cancellationToken = default);

        //Task<Reservation?> GetReservationWithDetailsAsync(Guid id, 
        //    CancellationToken cancellationToken = default);

        //عشان اجيب الحجز بالتفصيل

    
        Task<bool> IsRoomAvailableAsync(Guid roomId, DateOnly checkin, DateOnly checkout,
            Guid? excludeReservationId = null, CancellationToken cancellationToken = default);

       
        Task<Reservation?> GetEntityForUpdateAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
