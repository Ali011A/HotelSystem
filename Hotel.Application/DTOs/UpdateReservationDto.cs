using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs
{
    public class UpdateReservationDto
    {
        public Guid ReservationId { get; set; }
        [Required]
        public DateOnly CheckinDate { get; set; }
        [Required]
        public DateOnly CheckoutDate { get; set; }
        [Range(1, 10)]
        public int GuestCount { get; set; } = 1;
    }
}
