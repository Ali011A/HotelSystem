using Hotel.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public class User:BaseModel
    {
       

        [MaxLength(180)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int FailedLoginCount { get; set; }
        public DateTime? LockoutEnd { get; set; }

        // Navigation Properties (العلاقات)
        public UserType UserRoles { get; set; } 
        public HotelStaff? HotelStaff { get; set; } // 1-to-1 relationship (اختياري)
        public Customer? Customer { get; set; } // 1-to-1 relationship (اختياري)    
    }
}