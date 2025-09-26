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
    public class Room : BaseModel
    {
      
        public int RoomNumber { get; set; }

        public RoomType RoomType { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public int Capacity { get; set; }

        [MaxLength(50)]
        public RoomStatus AvailabilityStatus { get; set; } 
        [MaxLength(50)]
        public string? Description { get; set; } 
        public int Floor { get; set; }

        public int BedCount { get; set; }
       
        public ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();
        public ICollection<OfferRoom> OfferRooms { get; set; } = new List<OfferRoom>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public ICollection<AppUser> AppUsers { get; set; }


    }
}

