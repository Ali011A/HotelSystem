using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public class RoomFacility 
    {
        public Guid RoomId { get; set; }
        public  Room Room { get; set; }

        public Guid FacilityId { get; set; }
        public Facility Facility { get; set; }

    }
}
