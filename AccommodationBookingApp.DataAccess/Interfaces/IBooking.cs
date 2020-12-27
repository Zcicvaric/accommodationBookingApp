using AccommodationBookingApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IBooking
    {
        Task<Booking> CreateBookingAsync(int accommodationId, string userId, DateTime checkInDate, DateTime checkOutDate);
        Task<bool> DeleteBookingAsync(int bookingId);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<List<Booking>> GetAllPreviousStaysForUserAsync(string userId);
        Task<List<Booking>> GetAllUpcomingStaysForUserAsync(string userId);
        Task<List<Booking>> GetAllDeclinedStaysForUserAsync(string userId);
        Task<List<Booking>> GetAllCancelledStaysForUserAsync(string userId);
        Task<List<Booking>> GetAllBookingsForAccommodationAsync(int accommodationId);
        Task<List<Booking>> GetAllUpcomingBookingsForAccommodationAsync(int accommodationId);
        Task<List<Booking>> GetAllBookingsForHostAsync(string userId);
        Task<Booking> UpdateBookingStatusAsync(int bookingId, ApprovalStatus newApprovalStatus);
    }
}
