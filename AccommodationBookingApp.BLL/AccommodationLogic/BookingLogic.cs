using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.AccommodationLogic
{
    public class BookingLogic
    {

        IBooking _booking = new DataAccess.Functions.BookingFunctions();
        public async Task<Booking> CreateNewBooking(int accommodationId, string applicationUserId)
        {
            var booking = await _booking.CreateBooking(accommodationId, applicationUserId);

            return booking;
        }
    }
}
