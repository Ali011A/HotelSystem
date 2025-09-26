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
    public  class Payment : BaseModel
    {
      
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }


        public PaymentStatus PaymentStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string PaymentGateway { get; set; } = string.Empty;
        [MaxLength(50)]
        public string TransactionId { get; set; } = string.Empty;
        [MaxLength(10)]
        public string Currency { get; set; } = string.Empty;

        public DateTime RefundDate { get; set; }
        public string RecieveUrl { get; set; }


        [Column(TypeName = "decimal(18, 2)")]
        public decimal RefundAmount { get; set; }
        
    
    }
}
