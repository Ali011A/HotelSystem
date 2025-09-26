using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Shared.Enums
{
    public enum RoomType

    {
        Single,
        Double,
        Suite
    }

    public enum RoomStatus
    {
        Avaliable,
        Booked,
        Maintenance

 
    }

    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Canceled,
        Completed
    }

    public enum PaymentMethod
    {
        Card,
        Cach,
        Fawry
    }

    public enum PaymentStatus
    {
        ending,
        Completed,
        Failed,
        Refunded
    }
}
