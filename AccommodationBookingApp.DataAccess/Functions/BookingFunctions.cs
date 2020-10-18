using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
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
        public async Task<Booking> CreateBooking(int accommodationId, string userId)
        {
            Booking booking = new Booking { 
                                AccommodationId = accommodationId,
                                ApplicationUserId = userId,
                                DateOfArrival = DateTime.Now,
                                NumberOfDaysStaying = 2,
                                ApprovalStatus = false};
            
            await Context.Bookings.AddAsync(booking);
            await Context.SaveChangesAsync();

            return booking;
        }
    }
}
