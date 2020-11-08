using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Functions
{
    public class BookingFunctions : IBooking
    {
        private DatabaseContext Context;

        public BookingFunctions()
        {
            Context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions);
        }
        public async Task<Booking> CreateBooking(int accommodationId, string userId,
                                                 DateTime checkInDate, DateTime checkOutDate)
        { 
            Accommodation accommodation = await Context.Accommodations.Where(x => x.Id == accommodationId)
                                                                        .FirstOrDefaultAsync();
            ApplicationUser applicationUser = await Context.Users.Where(applicationUser => applicationUser.Id == userId)
                                                                        .FirstOrDefaultAsync();
            Booking booking = new Booking {
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

        public async Task <List<Booking>> GetAllPreviousStaysForUser(string userId)
        {
            //with the .include(FK entity) we include the objects related
            var allPreviousBookingWithUserId = await Context.Bookings.Include("Accommodation").Include("ApplicationUser").
                                                     Where(booking => booking.ApplicationUser.Id == userId
                                                     && booking.CheckInDate < DateTime.Now).ToListAsync();

            return allPreviousBookingWithUserId;
        }

        public async Task<List<Booking>> GetAllUpcomingStaysForUser(string userId)
        {
            //with the .include(FK entity) we include the objects related
            var allUpcomingBookingsWithUserId = await Context.Bookings.Include("Accommodation").Include("ApplicationUser").
                                                Where(booking => booking.ApplicationUser.Id == userId
                                                && booking.CheckInDate > DateTime.Now).ToListAsync();

            return allUpcomingBookingsWithUserId;
        }
    }
}
