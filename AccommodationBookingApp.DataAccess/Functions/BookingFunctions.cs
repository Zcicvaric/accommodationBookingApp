using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Functions
{
    public class BookingFunctions : IBooking
    {
        private readonly DatabaseContext Context;

        public BookingFunctions()
        {
            Context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions);
        }
        public async Task<Booking> CreateBookingAsync(int accommodationId, string userId,
                                                 DateTime checkInDate, DateTime checkOutDate)
        {
            var accommodation = await Context.Accommodations.Where(x => x.Id == accommodationId)
                                                                        .FirstOrDefaultAsync();
            var applicationUser = await Context.Users.Where(applicationUser => applicationUser.Id == userId)
                                                                        .FirstOrDefaultAsync();
            var booking = new Booking
            {
                Accommodation = accommodation,
                ApplicationUser = applicationUser,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                ApprovalStatus = accommodation.RequireApproval ? ApprovalStatus.Pending : ApprovalStatus.Approved
            };

            await Context.Bookings.AddAsync(booking);
            await Context.SaveChangesAsync();

            return booking;
        }

        public async Task<bool> DeleteBookingAsync(int bookingId)
        {
            var bookingToDelete = await Context.Bookings.Where(booking => booking.Id == bookingId).FirstOrDefaultAsync();

            Context.Bookings.Remove(bookingToDelete);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return false;
            }

            return true;
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            var booking = await Context.Bookings.Include("ApplicationUser").Include("Accommodation")
                                                    .Where(booking => booking.Id == bookingId)
                                                    .FirstOrDefaultAsync();

            return booking;
        }

        public async Task<List<Booking>> GetAllPreviousStaysForUserAsync(string userId)
        {
            //with the .include(FK entity) we include the objects related
            var allPreviousBookingWithUserId = await Context.Bookings.Include("Accommodation").Include("ApplicationUser").
                                                     Where(booking => booking.ApplicationUser.Id == userId
                                                     && booking.CheckInDate.Date < DateTime.Now.Date
                                                     && booking.ApprovalStatus == ApprovalStatus.Approved).
                                                     OrderBy(booking => booking.CheckInDate).
                                                     ToListAsync();

            return allPreviousBookingWithUserId;
        }

        public async Task<List<Booking>> GetAllUpcomingStaysForUserAsync(string userId)
        {
            //with the .include(FK entity) we include the objects related
            var allUpcomingBookingsWithUserId = await Context.Bookings.Include("Accommodation").Include("ApplicationUser").
                                                Where(booking => booking.ApplicationUser.Id == userId
                                                && booking.CheckInDate.Date >= DateTime.Now.Date
                                                && (booking.ApprovalStatus == ApprovalStatus.Pending
                                                || booking.ApprovalStatus == ApprovalStatus.Approved)).
                                                OrderBy(booking => booking.CheckInDate).
                                                ToListAsync();

            return allUpcomingBookingsWithUserId;
        }

        public async Task<List<Booking>> GetAllDeclinedStaysForUserAsync(string userId)
        {
            var allDeclinedBookingsWithUserId = await Context.Bookings.Include("Accommodation").Include("ApplicationUser").
                                                Where(booking => booking.ApplicationUser.Id == userId
                                                && booking.ApprovalStatus == ApprovalStatus.Declined).
                                                OrderBy(booking => booking.CheckInDate)
                                                .ToListAsync();

            return allDeclinedBookingsWithUserId;
        }

        public async Task<List<Booking>> GetAllCancelledStaysForUserAsync(string userId)
        {
            var allCancelledStaysForUser = await Context.Bookings.Include("Accommodation").Include("ApplicationUser").
                                           Where(booking => booking.ApplicationUser.Id == userId
                                           && (booking.ApprovalStatus == ApprovalStatus.Cancelled ||
                                               booking.ApprovalStatus == ApprovalStatus.CancelledByUser)).
                                               OrderBy(booking => booking.CheckInDate).
                                               ToListAsync();

            return allCancelledStaysForUser;
        }

        public async Task<List<Booking>> GetAllBookingsForAccommodationAsync(int accommodationId)
        {
            var bookingsForAccommodation = await Context.Bookings.Include("Accommodation").Include("ApplicationUser").
                                           Where(booking => booking.Accommodation.Id == accommodationId).
                                           OrderBy(booking => booking.CheckInDate).
                                           ToListAsync();

            return bookingsForAccommodation;
        }
        public async Task<List<Booking>> GetAllUpcomingBookingsForAccommodationAsync(int accommodationId)
        {
            var upcomingBookingsForAccommodation = await Context.Bookings.Include("Accommodation").
                                                   Where(booking => booking.Accommodation.Id == accommodationId
                                                   && (booking.ApprovalStatus == ApprovalStatus.Approved
                                                       || booking.ApprovalStatus == ApprovalStatus.Pending)
                                                   && booking.CheckInDate.Date >= DateTime.Now.Date).
                                                   OrderBy(booking => booking.CheckInDate).
                                                   ToListAsync();

            return upcomingBookingsForAccommodation;
        }

        public async Task<List<Booking>> GetAllBookingsForHostAsync(string userId)
        {
            var allBookingsWithHostId = await Context.Bookings.Include("Accommodation").Include("ApplicationUser").
                                        Where(booking => booking.Accommodation.ApplicationUser.Id == userId)
                                        .OrderBy(booking => booking.CheckInDate)
                                        .ToListAsync();

            return allBookingsWithHostId;
        }
        public async Task<Booking> UpdateBookingStatusAsync(int bookingId, ApprovalStatus newApprovalStatus)
        {
            var booking = await Context.Bookings.FindAsync(bookingId);

            if (booking != null)
            {
                booking.ApprovalStatus = newApprovalStatus;

                await Context.SaveChangesAsync();

            }
            return booking;
        }
    }
}
