using AutoMapper;
using Hotel.Api.ResponseViewModel;
using Hotel.Application.Common.Exceptions;
using Hotel.Application.DTOs;
using Hotel.Application.Interfaces.Queries;
using Hotel.Application.InterfaceServices;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Models;
using Hotel.Shared.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IReservationReadRepository _readRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(
            IUnitOfWork uow,
            IReservationReadRepository readRepo,
            IMapper mapper,
            ILogger<ReservationService> logger)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _readRepo = readRepo ?? throw new ArgumentNullException(nameof(readRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ReservationViewModel> CreateAsync(CreateReservationDto dto,
            CancellationToken ct = default)
        {
            // Validate dates
            if (dto.CheckinDate >= dto.CheckoutDate)
                throw new DomainValidationException("Checkout must be after checkin.");

            // compute nights count
            var nights = (dto.CheckoutDate.ToDateTime(TimeOnly.MinValue) - 
                dto.CheckinDate.ToDateTime(TimeOnly.MinValue)).Days;
            if (nights <= 0)
                throw new DomainValidationException("Invalid nights count.");

            // Start transaction through unit of work
            await _uow.BeginTransactionAsync(ct);
            try
            {
                // Load room (for price, capacity, currency)
//                var room = await _uow.Rooms.GetByIdAsync(dto.RoomId, ct)
//                           ?? throw new NotFoundException(nameof(Room), dto.RoomId);

//                // Validate guest count against room capacity
//                if (dto.GuestCount > room.Capacity)
//                    throw new DomainValidationException($@"Guest count ({dto.GuestCount}) 
//exceeds room capacity ({room.Capacity}).");

                // Availability check
                var available = await _uow.Reservations.IsRoomAvailableAsync(dto.RoomId, dto.CheckinDate, dto.CheckoutDate, null, ct);
                if (!available)
                    throw new ConflictException("Room is not available for the selected dates.");

                // Compute pricing (subtotal + taxes/fees — replace ComputeTaxes with your logic)
                // var subtotal = room.Price * nights;
                var subtotal = 750 * nights;
                var taxes = ComputeTaxes(subtotal);
                var total = decimal.Round(subtotal + taxes, 2, MidpointRounding.AwayFromZero);

                // Create entity
                var reservation = new Reservation
                {
                    RoomId = dto.RoomId,
                    CustomerId = dto.CustomerId,
                    CheckinDate = dto.CheckinDate,
                    CheckoutDate = dto.CheckoutDate,
                    BookingDate = DateTime.UtcNow,
                    GuestCount = dto.GuestCount,
                    Nights = nights,
                    TotalAmount = total,
                    Currency = "EGP"  ,//string.IsNullOrWhiteSpace(room.Currency) ? "EGP" : room.Currency,
                    Status = ReservationStatus.Confirmed
                };

                // Add and save
                await _uow.Reservations.AddAsync(reservation, ct);
                await _uow.SaveChangesAsync(ct);

                // Commit transaction
                await _uow.CommitTransactionAsync(ct);

                // Read projection (DTO) and map to ViewModel
                var details = await _readRepo.GetReservationDetailsAsync(reservation.Id, ct)
                              ?? throw new Exception("Failed to load created reservation.");

                _logger.LogInformation(@"Reservation {ReservationId} 
                     created for Room {RoomId} by Customer {CustomerId},
                reservation.Id, reservation.RoomId, reservation.CustomerId");

                return _mapper.Map<ReservationViewModel>(details);
            }
            catch
            {
                // Ensure rollback on any failure
                try { await _uow.RollbackTransactionAsync(ct); } catch { /* best-effort rollback */ }
                _logger.LogWarning("Create reservation failed for Room {RoomId} Customer {CustomerId}", dto.RoomId, dto.CustomerId);
                throw;
            }
        }

        public async Task<ReservationViewModel> 
            UpdateAsync(Guid id, UpdateReservationDto dto, CancellationToken ct = default)
        {
            if (dto.CheckinDate >= dto.CheckoutDate)
                throw new DomainValidationException("Checkout must be after checkin.");

            // Get tracked entity for update
            var existing = await _uow.Reservations.GetEntityForUpdateAsync(id, ct)
                           ?? throw new NotFoundException(nameof(Reservation), id);

            // compute nights
            var nights = (dto.CheckoutDate.ToDateTime(TimeOnly.MinValue)
                - dto.CheckinDate.ToDateTime(TimeOnly.MinValue)).Days;
            if (nights <= 0) throw new DomainValidationException("Invalid nights count.");

            // Load room to validate capacity and price
//            var room = await _uow.Rooms.GetByIdAsync(existing.RoomId, ct)
//                       ?? throw new NotFoundException(nameof(Room), existing.RoomId);

//            if (dto.GuestCount > room.Capacity)
//                throw new DomainValidationException($@"Guest count ({dto.GuestCount})
//exceeds room capacity ({room.Capacity}).");

            // Availability check excluding this reservation
            var available = await _uow.Reservations.IsRoomAvailableAsync(existing.RoomId, dto.CheckinDate,
                dto.CheckoutDate, existing.Id, ct);
            if (!available) throw new ConflictException("Room is not available for the selected dates.");

            // Apply updates
            existing.CheckinDate = dto.CheckinDate;
            existing.CheckoutDate = dto.CheckoutDate;
            existing.GuestCount = dto.GuestCount;
            existing.Nights = nights;
            existing.TotalAmount = 0; // decimal.Round(room.Price * nights, 2, MidpointRounding.AwayFromZero);

            _uow.Reservations.Update(existing);
            await _uow.SaveChangesAsync(ct);

            var details = await _readRepo.GetReservationDetailsAsync(existing.Id, ct)
                          ?? throw new Exception("Failed to reload reservation.");

            _logger.LogInformation("Reservation {ReservationId} updated", existing.Id);

            return _mapper.Map<ReservationViewModel>(details);
        }

        public async Task CancelAsync(Guid id, CancellationToken ct = default)
        {
            // Get tracked entity (so EF will save changes)
            var existing = await _uow.Reservations.GetEntityForUpdateAsync(id, ct)
                           ?? throw new NotFoundException(nameof(Reservation), id);

            existing.Status = ReservationStatus.Canceled;
            existing.CancellationDate = DateTime.UtcNow;

            _uow.Reservations.Update(existing);
            await _uow.SaveChangesAsync(ct);

            _logger.LogInformation("Reservation {ReservationId} canceled", id);
        }

        public async Task<ReservationViewModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var details = await _readRepo.GetReservationDetailsAsync(id, ct);
            if (details == null) return null;
            return _mapper.Map<ReservationViewModel>(details);
        }

        public async Task<PagedResponse<ReservationViewModel>> ListAsync(int page = 1, 
            int pageSize = 20, CancellationToken ct = default)
        {
            var (total, items) = await _readRepo.GetPagedReservationsAsync(page, pageSize, ct);
            var vms = items.Select(d => _mapper.Map<ReservationViewModel>(d)).ToList();

            return new PagedResponse<ReservationViewModel>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = vms
            };
        }

        // ---------- helper ----------
        private decimal ComputeTaxes(decimal subtotal)
        {
            const decimal taxRate = 0.00m;
            return decimal.Round(subtotal * taxRate, 2, MidpointRounding.AwayFromZero);
        }
    }
}

