using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Functions;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.AccommodationLogic
{
    public class BookingLogic
    {
        readonly IBooking bookingFunctions = new BookingFunctions();
        public async Task<Booking> CreateNewBookingAsync(int accommodationId, string applicationUserId,
                                                    DateTime checkInDate, DateTime checkOutDate)
        {
            var booking = await bookingFunctions.CreateBookingAsync(accommodationId, applicationUserId,
                                                       checkInDate, checkOutDate);

            return booking;
        }

        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            return await bookingFunctions.DeleteBookingAsync(bookingId);
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await bookingFunctions.GetBookingByIdAsync(bookingId);
        }

        public async Task<List<Booking>> GetAllPreviousStaysForUserAsync(string applicationUserId)
        {
            var previousStays = await bookingFunctions.GetAllPreviousStaysForUserAsync(applicationUserId);

            return previousStays;
        }

        public async Task<List<Booking>> GetAllUpcomingStaysForUserAsync(string applicationUserId)
        {
            var upcomingStays = await bookingFunctions.GetAllUpcomingStaysForUserAsync(applicationUserId);

            return upcomingStays;
        }

        public async Task<List<Booking>> GetAllDeclinedStaysForUserAsync(string applicationUserId)
        {
            var declinedStays = await bookingFunctions.GetAllDeclinedStaysForUserAsync(applicationUserId);

            return declinedStays;
        }

        public async Task<List<Booking>> GetAllCancelledStaysForUserAsync(string applicationUserId)
        {
            var cancelledStays = await bookingFunctions.GetAllCancelledStaysForUserAsync(applicationUserId);

            return cancelledStays;
        }

        public async Task<List<Booking>> GetAllBookingsForAccommodationAsync(int accommodationId)
        {
            var bookings = await bookingFunctions.GetAllBookingsForAccommodationAsync(accommodationId);

            return bookings;
        }
        public async Task<List<Booking>> GetAllPendingReservationsForHostAsync(string userId)
        {
            var bookingsForHost = await bookingFunctions.GetAllBookingsForHostAsync(userId);

            var pendingReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Pending)
                                      .OrderBy(booking => booking.CheckInDate).ToList();

            return pendingReservations;
        }
        public async Task<List<Booking>> GetAllApprovedReservationsForHostAsync(string userId)
        {
            var bookingsForHost = await bookingFunctions.GetAllBookingsForHostAsync(userId);

            var approvedReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Approved
                                                             && booking.CheckInDate.Date >= DateTime.Now.Date)
                                                             .OrderBy(booking => booking.CheckInDate).ToList();

            return approvedReservations;
        }
        public async Task<List<Booking>> GetAllPreviousReservationsForHostAsync(string userId)
        {
            var bookingsForHost = await bookingFunctions.GetAllBookingsForHostAsync(userId);

            var previousReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Approved
                                                             && booking.CheckOutDate.Date <= DateTime.Now.Date)
                                                             .ToList();

            return previousReservations;
        }
        public async Task<List<Booking>> GetAllDeclinedReservationsForHostAsync(string userId)
        {
            var bookingsForHost = await bookingFunctions.GetAllBookingsForHostAsync(userId);

            var declinedReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Declined)
                                                             .ToList();

            return declinedReservations;
        }
        public async Task<List<Booking>> GetAllCancelledReservationsForHostAsync(string userId)
        {
            var bookingsForHost = await bookingFunctions.GetAllBookingsForHostAsync(userId);

            var canceledReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Cancelled)
                                                             .ToList();

            return canceledReservations;
        }
        public async Task<List<Booking>> GetAllCancelledByUserReservationsForHostAsync(string userId)
        {
            var bookingsForHost = await bookingFunctions.GetAllBookingsForHostAsync(userId);

            var cancelledByUserReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.CancelledByUser)
                                                                   .ToList();

            return cancelledByUserReservations;
        }
        public async Task<bool> ApproveBookingAsync(int bookingId)
        {
            Booking updatedBooking;
            try
            {
                updatedBooking = await bookingFunctions.UpdateBookingStatusAsync(bookingId, ApprovalStatus.Approved);
            }
            catch
            {
                return false;
            }
            if (updatedBooking.ApprovalStatus == ApprovalStatus.Approved)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeclineBookingAsync(int bookingId)
        {
            Booking updatedBooking;
            try
            {
                updatedBooking = await bookingFunctions.UpdateBookingStatusAsync(bookingId, ApprovalStatus.Declined);
            }
            catch
            {
                return false;
            }
            if (updatedBooking.ApprovalStatus == ApprovalStatus.Declined)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            Booking updatedBooking;
            try
            {
                updatedBooking = await bookingFunctions.UpdateBookingStatusAsync(bookingId, ApprovalStatus.Cancelled);
            }
            catch
            {
                return false;
            }
            if (updatedBooking.ApprovalStatus == ApprovalStatus.Cancelled)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CancelBookingAsUserAsync(int bookingId)
        {
            Booking updatedBooking;

            try
            {
                updatedBooking = await bookingFunctions.UpdateBookingStatusAsync(bookingId, ApprovalStatus.CancelledByUser);
            }
            catch
            {

                return false;
            }
            if (updatedBooking.ApprovalStatus == ApprovalStatus.CancelledByUser)
            {
                return true;
            }

            return false;
        }

        public async Task<List<Booking>> GetAllPendingReservationsForAccommodationAsync(int accommodationId)
        {
            var allBookingsForAccommodation = await bookingFunctions.GetAllBookingsForAccommodationAsync(accommodationId);

            var pendingBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Pending)
                                                                    .ToList();

            return pendingBookings;
        }

        public async Task<List<Booking>> GetAllApprovedReservationsForAccommodationAsync(int accommodationId)
        {
            var allBookingsForAccommodation = await bookingFunctions.GetAllBookingsForAccommodationAsync(accommodationId);

            var approvedBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Approved)
                                                                    .ToList();
            return approvedBookings;
        }

        public async Task<List<Booking>> GetAllPreviousReservationsForAccommodationAsync(int accommodationId)
        {
            var allBookingsForAccommodation = await bookingFunctions.GetAllBookingsForAccommodationAsync(accommodationId);

            var previousBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Approved
                                                                               && booking.CheckInDate.Date <= DateTime.Now.Date)
                                                                               .ToList();
            return previousBookings;
        }

        public async Task<List<Booking>> GetAllDeclinedReservationsForAccommodationAsync(int accommodationId)
        {
            var allBookingsForAccommodation = await bookingFunctions.GetAllBookingsForAccommodationAsync(accommodationId);

            var declinedBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Declined)
                                                                    .ToList();

            return declinedBookings;
        }

        public async Task<List<Booking>> GetAllCancelledReservationsForAccommodationAsync(int accommodationId)
        {
            var allBookingsForAccommodation = await bookingFunctions.GetAllBookingsForAccommodationAsync(accommodationId);

            var cancelledBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Declined)
                                                                      .ToList();

            return cancelledBookings;
        }

        public async Task<List<Booking>> GetAllCancelledByUserReservationsForAccommodationAsync(int accommodationId)
        {
            var allBookingsForAccommodation = await bookingFunctions.GetAllBookingsForAccommodationAsync(accommodationId);

            var cancelledByUserBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.CancelledByUser)
                                                                            .ToList();

            return cancelledByUserBookings;
        }

        public async Task<List<Booking>> GetAllUpcomingApprovedOrPendingReservationsForAccommodationAsync(int accommodationId)
        {
            var allBookingsForAccommodation = await bookingFunctions.GetAllBookingsForAccommodationAsync(accommodationId);

            var upcomingBookings = allBookingsForAccommodation.Where(booking => (booking.ApprovalStatus == ApprovalStatus.Approved
                                                                                           || booking.ApprovalStatus == ApprovalStatus.Pending)
                                                                                           && booking.CheckInDate > DateTime.Now.Date).ToList();

            return upcomingBookings;
        }
    }
}
