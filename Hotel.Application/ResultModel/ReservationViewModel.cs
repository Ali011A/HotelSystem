using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs
{
    public class ReservationViewModel
    {

         public Guid Id { get; set; }
        public int RoomNumber { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public DateOnly CheckinDate { get; set; }
        public DateOnly CheckoutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "EGP";
        public DateTime BookingDate { get; set; }
        public int Nights { get; set; }
    }
}
