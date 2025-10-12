using Hotel.Api.ResponseViewModel;
using Hotel.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.InterfaceServices
{
    public interface IReservationService
    {
        Task<ReservationViewModel> CreateAsync(CreateReservationDto dto, CancellationToken ct = default);
       Task<ReservationViewModel> UpdateAsync(Guid id, UpdateReservationDto dto, CancellationToken ct = default);
       Task CancelAsync(Guid id, CancellationToken ct = default);
      Task<ReservationViewModel?> GetByIdAsync(Guid id, CancellationToken ct = default);
     Task<PagedResponse<ReservationViewModel>> ListAsync(int page = 1, int pageSize = 20, CancellationToken ct = default);
    }
}
