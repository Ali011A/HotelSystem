using Hotel.Application.DTOs;
using Hotel.Application.Interfaces.Queries;
using Hotel.Domain.Interfaces.Repositories;
using Hotel.Domain.Models;
using Hotel.Infrastructure.Persistence;
using Hotel.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Repositories
{
    public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository, IReservationReadRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Domain method (no DTO)
        public async Task<bool> IsRoomAvailableAsync(Guid roomId, DateOnly checkin, DateOnly checkout, Guid? excludeReservationId = null, CancellationToken cancellationToken = default)
        {
            var query = QueryAsNoTracking().Where(r =>
                r.RoomId == roomId &&
                r.Status != ReservationStatus.Canceled &&
                r.CheckinDate < checkout &&
                checkin < r.CheckoutDate);

            if (excludeReservationId.HasValue)
                query = query.Where(r => r.Id != excludeReservationId.Value);

            var anyOverlap = await query.AnyAsync(cancellationToken);
            return !anyOverlap;
        }

        // Domain: tracked entity for updates
        public async Task<Reservation?> GetEntityForUpdateAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Query().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        // Application/read:
        public async Task<ReservationDetailsDto?> GetReservationDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var q = from r in QueryAsNoTracking()
                    join room in _context.Rooms.AsNoTracking() on r.RoomId equals room.Id
                    join cust in _context.Customers.AsNoTracking() on r.CustomerId equals cust.UserId
                    where r.Id == id
                    select new ReservationDetailsDto
                    {
                        Id = r.Id,
                        RoomNumber = room.RoomNumber,
                        CustomerName = (cust.FirstName ?? "") + " " + (cust.LastName ?? ""),
                        Status = r.Status.ToString(),
                        CheckinDate = r.CheckinDate,
                        CheckoutDate = r.CheckoutDate,
                        TotalAmount = r.TotalAmount,
                        Currency = r.Currency,
                        BookingDate = r.BookingDate,
                        Nights = r.Nights,
                        GuestCount = r.GuestCount
                    };

            return await q.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<(int Total, List<ReservationDetailsDto> Items)> GetPagedReservationsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var baseQ = from r in QueryAsNoTracking()
                        join room in _context.Rooms.AsNoTracking() on r.RoomId equals room.Id
                        join cust in _context.Customers.AsNoTracking() on r.CustomerId equals cust.UserId
                        select new { r, room, cust };

            var ordered = baseQ.OrderByDescending(x => x.r.BookingDate);

            var total = await ordered.CountAsync(cancellationToken);

            var items = await ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ReservationDetailsDto
                {
                    Id = x.r.Id,
                    RoomNumber = x.room.RoomNumber,
                    CustomerName = (x.cust.FirstName ?? "") + " " + (x.cust.LastName ?? ""),
                    Status = x.r.Status.ToString(),
                    CheckinDate = x.r.CheckinDate,
                    CheckoutDate = x.r.CheckoutDate,
                    TotalAmount = x.r.TotalAmount,
                    Currency = x.r.Currency,
                    BookingDate = x.r.BookingDate,
                    Nights = x.r.Nights,
                    GuestCount = x.r.GuestCount
                })
                .ToListAsync(cancellationToken);

            return (total, items);
        }


    }
}
