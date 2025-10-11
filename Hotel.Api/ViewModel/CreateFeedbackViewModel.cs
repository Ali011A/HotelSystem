using System.ComponentModel.DataAnnotations;

namespace Hotel.Api.ViewModel
{
    public class CreateFeedbackViewModel
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
