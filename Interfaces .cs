using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management__Debugging_
{
    //Interfaces Used Inside the Check Class

    public interface LCheckDate
    {
        void CheckInDate(string Id);
        void CheckOutDate(string Id);
    }

    public interface LCheckTime
    {
        void CheckInTime(string Id);
        void CheckOutTime(string Id);
    }

    public interface LUpdateCheckDate
    {
        void CheckInTime(string Id);
        void CheckOutTime(string Id);
    }

    public interface LBill
    {
        void Bill();
    }
}
