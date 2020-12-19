using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Functions;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.AccommodationLogic
{
    public class BookingLogic
    {

        IBooking _booking = new DataAccess.Functions.BookingFunctions();
        public async Task<Booking> CreateNewBooking(int accommodationId, string applicationUserId,
                                                    DateTime checkInDate, DateTime checkOutDate)
        {
            var booking = await _booking.CreateBooking(accommodationId, applicationUserId,
                                                       checkInDate, checkOutDate);

            return booking;
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _booking.GetBookingByIdAsync(bookingId);
        }

        public async Task <List<Booking>> GetAllPreviousStaysForUser(string applicationUserId)
        {
            var previousStays = await _booking.GetAllPreviousStaysForUser(applicationUserId);

            var previousStaysSorted = previousStays.OrderBy(previousStay => previousStay.CheckInDate).ToList();

            return previousStaysSorted;
        }

        public async Task <List<Booking>> GetAllUpcomingStaysForUser(string applicationUserId)
        {
            var upcomingStays = await _booking.GetAllUpcomingStaysForUser(applicationUserId);

            var upcomingStaysSorted = upcomingStays.OrderBy(upcomingStay => upcomingStay.CheckInDate).ToList();

            return upcomingStaysSorted;
        }

        public async Task <List<Booking>> GetAllDeclinedStaysForUser(string applicationUserId)
        {
            var declinedStays = await _booking.GetAllDeclinedStaysForUser(applicationUserId);

            var declinedStaysSorted = declinedStays.OrderBy(declinedStay => declinedStay.CheckInDate).ToList();

            return declinedStays;
        }

        public async Task <List<Booking>> GetAllCancelledStaysForUser(string applicationUserId)
        {
            var cancelledStays = await _booking.GetAllCancelledStaysForUser(applicationUserId);

            var cancelledStaysSorted = cancelledStays.OrderBy(cancelledStay => cancelledStay.CheckInDate).ToList();

            return cancelledStays;
        }

        public async Task <List<Booking>> GetAllBookingsForAccommodation(int accommodationId)
        {
            var bookings = await _booking.GetAllBookingsForAccommodation(accommodationId);

            var bookingsSorted = bookings.OrderBy(booking => booking.CheckInDate).ToList();

            return bookings;
        }
        public async Task<List<Booking>> GetAllPendingReservationsForHost(string userId)
        {
            var bookingsForHost = await _booking.GetAllBookingsForHost(userId);

            var pendingReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Pending)
                                      .OrderBy(booking => booking.CheckInDate).ToList();

            return pendingReservations;
        }
        public async Task<List<Booking>> GetAllApprovedReservationsForHost(string userId)
        {
            var bookingsForHost = await _booking.GetAllBookingsForHost(userId);

            var approvedReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Approved
                                                             && booking.CheckInDate.Date >= DateTime.Now.Date)
                                                             .OrderBy(booking => booking.CheckInDate).ToList();

            return approvedReservations;
        }
        public async Task<List<Booking>> GetAllPreviousReservationsForHost(string userId)
        {
            var bookingsForHost = await _booking.GetAllBookingsForHost(userId);

            var previousReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Approved
                                                             && booking.CheckOutDate.Date <= DateTime.Now.Date)
                                                             .OrderBy(booking => booking.CheckInDate).ToList();

            return previousReservations;
        }
        public async Task<List<Booking>> GetAllDeclinedReservationsForHost(string userId)
        {
            var bookingsForHost = await _booking.GetAllBookingsForHost(userId);

            var declinedReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Declined)
                                                            .OrderBy(booking => booking.CheckInDate).ToList();

            return declinedReservations;
        }
        public async Task<List<Booking>> GetAllCancelledReservationsForHost(string userId)
        {
            var bookingsForHost = await _booking.GetAllBookingsForHost(userId);

            var canceledReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.Cancelled)
                                                            .OrderBy(booking => booking.CheckInDate).ToList();

            return canceledReservations;
        }
        public async Task<List<Booking>> GetAllCancelledByUserReservationsForHost(string userId)
        {
            var bookingsForHost = await _booking.GetAllBookingsForHost(userId);

            var cancelledByUserReservations = bookingsForHost.Where(booking => booking.ApprovalStatus == ApprovalStatus.CancelledByUser)
                                                                    .OrderBy(booking => booking.CheckInDate).ToList();

            return cancelledByUserReservations;
        }
        public async Task<Boolean> ApproveBooking(int bookingId)
        {
            Booking updatedBooking;
            try
            {
                updatedBooking = await _booking.UpdateBookingStatus(bookingId, ApprovalStatus.Approved);
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
        public async Task<Boolean> DeclineBooking(int bookingId)
        {
            Booking updatedBooking;
            try
            {
                updatedBooking = await _booking.UpdateBookingStatus(bookingId, ApprovalStatus.Declined);
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
        public async Task<Boolean> CancelBooking(int bookingId)
        {
            Booking updatedBooking;
            try
            {
                updatedBooking = await _booking.UpdateBookingStatus(bookingId, ApprovalStatus.Cancelled);
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
        public async Task<Boolean> CancelBookingAsUser(int bookingId)
        {
            Booking updatedBooking;

            try
            {
                updatedBooking = await _booking.UpdateBookingStatus(bookingId, ApprovalStatus.CancelledByUser);
            }
            catch
            {

                return false;
            }
            if(updatedBooking.ApprovalStatus == ApprovalStatus.CancelledByUser)
            {
                return true;
            }

            return false;
        }
        public async Task <List<Booking>> GetAllPendingReservationsForAccommodation(int accommodationId)
        {
            List<Booking> allBookingsForAccommodation = await _booking.GetAllBookingsForAccommodation(accommodationId);

            List<Booking> pendingBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Pending)
                                                                             .OrderBy(booking => booking.CheckInDate).ToList();

            return pendingBookings;
        }

        public async Task<List<Booking>> GetAllApprovedReservationsForAccommodation(int accommodationId)
        {
            List<Booking> allBookingsForAccommodation = await _booking.GetAllBookingsForAccommodation(accommodationId);

            List<Booking> approvedBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Approved)
                                                                     .OrderBy(booking => booking.CheckInDate).ToList();
            return approvedBookings;
        }

        public async Task<List<Booking>> GetAllPreviousReservationsForAccommodation(int accommodationId)
        {
            List<Booking> allBookingsForAccommodation = await _booking.GetAllBookingsForAccommodation(accommodationId);

            List<Booking> previousBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Approved
                                                                               && booking.CheckInDate.Date <= DateTime.Now.Date)
                                                                               .OrderBy(booking => booking.CheckInDate).ToList();
            return previousBookings;
        }

        public async Task<List<Booking>> GetAllDeclinedReservationsForAccommodation(int accommodationId)
        {
            List<Booking> allBookingsForAccommodation = await _booking.GetAllBookingsForAccommodation(accommodationId);

            List<Booking> declinedBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Declined)
                                                                              .OrderBy(booking => booking.CheckInDate).ToList();

            return declinedBookings;
        }

        public async Task<List<Booking>> GetAllCancelledReservationsForAccommodation(int accommodationId)
        {
            List<Booking> allBookingsForAccommodation = await _booking.GetAllBookingsForAccommodation(accommodationId);

            List<Booking> cancelledBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.Declined)
                                                                              .OrderBy(booking => booking.CheckInDate).ToList();

            return cancelledBookings;
        }

        public async Task<List<Booking>> GetAllCancelledByUserReservationsForAccommodation(int accommodationId)
        {
            List<Booking> allBookingsForAccommodation = await _booking.GetAllBookingsForAccommodation(accommodationId);

            List<Booking> cancelledByUserBookings = allBookingsForAccommodation.Where(booking => booking.ApprovalStatus == ApprovalStatus.CancelledByUser)
                                                                                    .OrderBy(booking => booking.CheckInDate).ToList();

            return cancelledByUserBookings;
        }

        public async Task<List<Booking>> GetAllUpcomingApprovedOrPendingReservationsForAccommodation(int accommodationId)
        {
            List<Booking> allBookingsForAccommodation = await _booking.GetAllBookingsForAccommodation(accommodationId);

            List<Booking> upcomingBookings = allBookingsForAccommodation.Where(booking => (booking.ApprovalStatus == ApprovalStatus.Approved
                                                                                           || booking.ApprovalStatus == ApprovalStatus.Pending)
                                                                                           && booking.CheckInDate > DateTime.Now.Date).ToList();

            return upcomingBookings;
        }
    }
}
