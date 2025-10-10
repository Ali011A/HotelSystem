using Hotel.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Queries
{
    public interface IReservationReadRepository
    {
        Task<ReservationDetailsDto?> GetReservationDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<(int Total, List<ReservationDetailsDto> Items)> GetPagedReservationsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    }
}
