using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public class Customer
    {
        public  Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
