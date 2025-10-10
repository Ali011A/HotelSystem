using Hotel.Application.DTOs.OfferDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Interfaces
{
    public interface IOfferService
    {

        Task<OfferResponseDto> AddOfferAsync(OfferCreateDto dto, Guid staffId);
        Task<OfferResponseDto?> UpdateOfferAsync(Guid id, OfferUpdateDto dto);
        Task<IEnumerable<OfferResponseDto>> GetAllOffersAsync();
        Task<OfferResponseDto?> GetOfferByIdAsync(Guid id);
        Task<bool> DeleteOfferAsync(Guid id);
    }

}
