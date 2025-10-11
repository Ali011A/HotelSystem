using System.ComponentModel.DataAnnotations;

namespace Hotel.Api.ViewModel
{
    public class UpdateFeedbackViewModel
    {

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string Comment { get; set; }
    }
}
