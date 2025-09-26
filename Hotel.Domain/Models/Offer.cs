using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public  class Offer : BaseModel
    {
     
        [MaxLength(255)]
        public string OfferName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        [Column(TypeName = "decimal(5, 2)")] // e.g. 99.99
        public decimal Discount { get; set; }
       // public bool IsActive { get; set; }
        public int MinimumNights { get; set; }
    
      
        public ICollection<OfferRoom> OfferRooms { get; set; } = new List<OfferRoom>();
    }
}
