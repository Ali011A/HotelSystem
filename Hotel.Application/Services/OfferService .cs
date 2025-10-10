using Hotel.Application.DTOs.OfferDTOs;
using Hotel.Domain.Interfaces;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;

        public OfferService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<OfferResponseDto> AddOfferAsync(OfferCreateDto dto, Guid staffId)
        {
            var offer = new Offer
            {
                Id = Guid.NewGuid(),
                OfferName = dto.OfferName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Discount = dto.Discount,
                MinimumNights = dto.MinimumNights,
               
                //StaffId = staffId,
                StaffId = null,
          

                OfferRooms = dto.AssignedRoomIds.Select(rid => new OfferRoom { RoomId = rid }).ToList()
            };

      

            await _offerRepository.AddAsync(offer);

            return new OfferResponseDto
            {
                Id = offer.Id,
                OfferName = offer.OfferName,
                Discount = offer.Discount,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                RoomIds = offer.OfferRooms.Select(or => or.RoomId).ToList()
            };
        }

        public async Task<OfferResponseDto?> UpdateOfferAsync(Guid id, OfferUpdateDto dto)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer == null) return null;

            offer.OfferName = dto.OfferName;
            offer.StartDate = dto.StartDate;
            offer.EndDate = dto.EndDate;
            offer.Discount = dto.Discount;
            offer.MinimumNights = dto.MinimumNights;
            offer.OfferRooms = dto.AssignedRoomIds.Select(rid => new OfferRoom { RoomId = rid, OfferId = offer.Id }).ToList();

            await _offerRepository.UpdateAsync(offer);

            return new OfferResponseDto
            {
                Id = offer.Id,
                OfferName = offer.OfferName,
                Discount = offer.Discount,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                RoomIds = offer.OfferRooms.Select(or => or.RoomId).ToList()
            };
        }

        public async Task<IEnumerable<OfferResponseDto>> GetAllOffersAsync()
        {
            var offers = await _offerRepository.GetAllAsync();

            return offers.Select(o => new OfferResponseDto
            {
                Id = o.Id,
                OfferName = o.OfferName,
                Discount = o.Discount,
                StartDate = o.StartDate,
                EndDate = o.EndDate,
                RoomIds = o.OfferRooms.Select(or => or.RoomId).ToList()
            }).ToList();
        }

        public async Task<OfferResponseDto?> GetOfferByIdAsync(Guid id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer == null) return null;

            return new OfferResponseDto
            {
                Id = offer.Id,
                OfferName = offer.OfferName,
                Discount = offer.Discount,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                RoomIds = offer.OfferRooms.Select(or => or.RoomId).ToList()
            };
        }

        public async Task<bool> DeleteOfferAsync(Guid id)
        {
            var offer = await _offerRepository.GetByIdAsync(id);
            if (offer == null) return false;

            await _offerRepository.SoftDeleteAsync(offer);
            return true;
        }
    }

}
