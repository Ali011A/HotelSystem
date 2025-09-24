using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public class OfferRoom
    {
        public Guid OfferId { get; set; }
        
        public Offer Offer { get; set; } = null!;

        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;
    }
}
