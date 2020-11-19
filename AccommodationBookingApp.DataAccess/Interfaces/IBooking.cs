using AccommodationBookingApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IBooking
    {
        Task <Booking> CreateBooking(int accommodationId, string userId, DateTime checkInDate, DateTime checkOutDate);
        Task <List<Booking>> GetAllPreviousStaysForUser(string userId);
        Task <List<Booking>> GetAllUpcomingStaysForUser(string userId);
        Task<List<Booking>> GetAllDeclinedStaysForUser(string userId);
        Task<List<Booking>> GetAllCancelledStaysForUser(string userId);
        Task <List<Booking>> GetAllBookingsForAccommodation(int accommodationId);
        Task <List<Booking>> GetAllBookingsForHost(string userId);
        Task <Booking> UpdateBookingStatus(int bookingId, ApprovalStatus newApprovalStatus);
    }
}
