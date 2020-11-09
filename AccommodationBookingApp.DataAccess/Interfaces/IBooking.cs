using AccommodationBookingApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IBooking
    {
        Task<Booking> CreateBooking(int accommodationId, string userId, DateTime checkInDate, DateTime checkOutDate);
        Task <List<Booking>> GetAllPreviousStaysForUser(string userId);
        Task <List<Booking>> GetAllUpcomingStaysForUser(string userId);
        Task<List<Booking>> GetAllBookingsForAccommodation(int accommodationId);
    }
}
