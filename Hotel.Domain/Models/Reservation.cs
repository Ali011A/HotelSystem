using Hotel.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public class Reservation : BaseModel
    {
        public Guid RoomId { get; set; }

        public Room room { get; set; } 
        
        public ReservationStatus Status { get; set; }
        public DateOnly CheckinDate { get; set; }
        public DateOnly CheckoutDate { get; set; }
        public DateTime BookingDate { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty;

        public int GuestCount { get; set; }

        [MaxLength(500)]
        public string? SpecialRequests { get; set; }

        public DateTime? CancellationDate { get; set; }

        [MaxLength(1000)]
        public string? CancellationReason { get; set; }

        public int Nights { get; set; }

        public Feedback Feedback { get; set; }  
        public Payment Payments { get; set; } 
    }
}
