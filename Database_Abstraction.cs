using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management__Debugging_
{
    
    //Abstract Class For the Database
    public abstract class DataBase
    {

        //Guest related Methods
        public abstract void InsertGuestInfo(string firstname, string lastName, string phone_number, string id);
        public abstract void DeleteGuestInfo(string id);
        public abstract void DeleteReservation(string id);


        //Reservation Methods
        public abstract void CheckInDate(DateOnly checkInDate, string Id);
        public abstract void CheckOutDate(DateOnly DepartureDate, string id);


        //Hotel Check In-Out
        public abstract void UpdateArrivalTime(TimeSpan arrivalTime, string id, string status);
        public abstract void UpdateDepartureTime(TimeSpan DepartureTime, string id);
        public abstract void UpdateArrivalStatus(string status, string id);


        //Editing Reservation
        public abstract void UpdateCheckInDate(string id, DateOnly newCheckInDate);
        public abstract void UpdateCheckOutDate(string id, DateOnly newCheckOutDate);


        //Occupying Rooms 
        public abstract bool IsRoomOccupiedForDates(int roomNumber, DateTime checkInDateTime, DateTime checkOutDateTime);
        public abstract void UpdateRoomNumber(string id, int roomNumber, string roomtype);
        public abstract void UpdateRoomNumber_Guestinfo(string id, int roomNumber);


    }

}
