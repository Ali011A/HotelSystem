using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Domain.Models
{
    public class Facility : BaseModel
    {
        public string FacilityName { get; set; } 
        public string? Description { get; set; }
    }
}
