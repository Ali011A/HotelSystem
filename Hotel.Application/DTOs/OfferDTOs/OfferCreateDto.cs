using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.OfferDTOs
{
    public class OfferCreateDto
    {
        public string OfferName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal Discount { get; set; }
        public int MinimumNights { get; set; }
        public List<Guid> AssignedRoomIds { get; set; } = new();
    }
}
