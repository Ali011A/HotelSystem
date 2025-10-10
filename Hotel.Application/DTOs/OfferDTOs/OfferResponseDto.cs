using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.OfferDTOs
{
    public class OfferResponseDto
    {
        public Guid Id { get; set; }
        public string OfferName { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public List<Guid> RoomIds { get; set; } = new();
    }
}
