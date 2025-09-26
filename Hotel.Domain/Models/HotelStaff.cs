using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public class HotelStaff
    {
        public  Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string? Position { get; set; }
        public string? AccessRights { get; set; }

        public ICollection<Room> ManagedRooms { get; set; } = new List<Room>();
    }
}
