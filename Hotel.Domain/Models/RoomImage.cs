using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public class RoomImage : BaseModel
    {
        public Guid RoomId { get; set; }
        public Room Room { get; set; }

        public string Url { get; set; }
        public bool IsPrimary { get; set; } 
       
    }
}
