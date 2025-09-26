using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public  class Feedback: BaseModel
    {
        //public int CustomerId { get; set; }

        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }
       
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty; // Text type in DB
        public DateTime SubmissionDate { get; set; }

        [MaxLength(100)]
        public string? ResponseStaff { get; set; }
        public DateTime? ResponseDate { get; set; }

        // Navigation Properties
        //public Customer Customer { get; set; } = null!;

      
    }
}
