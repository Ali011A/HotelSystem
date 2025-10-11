using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Shared.Enums
{
    public enum StatusCode
    {

        Succes = 0,
        Faild = 1,
        Invalid = 2,
        NotFound = 3,
        Duplicate = 4,
        Unauthorized = 5,
        DataBaseError = 6,
        Created = 7,
        Updated = 8,
        Deleted = 9,
        FailedDeleting = 10,
        FailedCreating = 11,
        FailedUpdating = 12,
    }
}
