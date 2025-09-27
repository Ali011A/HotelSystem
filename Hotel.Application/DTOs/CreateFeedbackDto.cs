using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs
{
    public class CreateFeedbackDto
    {

        [Required]
        public Guid ReservationId { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }
}
