using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs
{
    public class CreateReservationDto
    {
        [Required] 
        public Guid RoomId { get; set; }
        [Required] 
        public Guid CustomerId { get; set; }
        [Required] 
        public DateOnly CheckinDate { get; set; }
        [Required] 
        public DateOnly CheckoutDate { get; set; }
        [Range(1, 10)] 
        public int GuestCount { get; set; }
    }
}
