using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Functions
{
    public class AccommodationFunctions : IAccommodation
    {
        private DatabaseContext Context;
        public AccommodationFunctions()
        {
            Context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions);
        }
        public async Task<Accommodation> CreateAccommodation(Accommodation accommodation,
                                                             int accommodationTypeId,
                                                             string applicationUserId)
        {

            AccommodationType accommodationType = await Context.AccommodationType.Where(accommodationType =>
                                                                                  accommodationType.Id == accommodationTypeId)
                                                                                  .FirstOrDefaultAsync();

            ApplicationUser applicationUser = await Context.Users.Where(applicationUser =>
                                                                  applicationUser.Id == applicationUserId)
                                                                  .FirstOrDefaultAsync();

            accommodation.AccommodationType = accommodationType;
            accommodation.ApplicationUser = applicationUser;


            await Context.Accommodations.AddAsync(accommodation);
            await Context.SaveChangesAsync();

            return accommodation;
        }

        public async Task<List<Accommodation>> GetAllAccommodations()
        {
            List<Accommodation> accommodations = new List<Accommodation>();
            accommodations = await Context.Accommodations.Include("AccommodationType").ToListAsync();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetAccommodationsWithUserIdAsync(string userId)
        {
            List<Accommodation> accommodations;
            accommodations = await Context.Accommodations.Include("AccommodationType").Include("ApplicationUser")
                                                              .Where(accommodation => accommodation.ApplicationUser.Id == userId).ToListAsync();

            return accommodations;
        }

        //mos na ovo napravit prezentaciju kako radi 
        public async Task<List<Accommodation>> GetFilteredAccommodations(string accommodationCity, int accommodationTypeId, int numberOfGuests,
                                                                         DateTime checkInDate, DateTime checkOutDate)
        {
            List<Accommodation> accommodations;
            List<Accommodation> listOfAccommodationsToRemove = new List<Accommodation>();
            accommodations = await Context.Accommodations.Include("AccommodationType").Include("ApplicationUser")
                                                                .Where(accommodation =>
                                                                accommodation.City == accommodationCity &&
                                                                accommodation.AccommodationType.Id == accommodationTypeId
                                                                && accommodation.NumberOfBeds >= numberOfGuests).ToListAsync();
            

            //obavezno provjerit rubne slucajeve npr kad gleda zadnju rezervaciju i sl++ (search dobro radi, problem sa check-in i check-out datumima na details stranici)
            foreach (Accommodation accommodation in accommodations)
            {
                var bookingsForAccommodation = await Context.Bookings.Where(booking => booking.Accommodation.Id == accommodation.Id).ToListAsync();
                if (bookingsForAccommodation.Count != 0)
                {
                    
                    foreach(Booking booking in bookingsForAccommodation)
                    {
                        if (checkInDate < booking.CheckOutDate && checkOutDate > booking.CheckInDate)
                        {
                            listOfAccommodationsToRemove.Add(accommodation);
                        }
                    }
                }
            }

            foreach(Accommodation accommodationToRemove in listOfAccommodationsToRemove)
            {
                accommodations.Remove(accommodationToRemove);
            }
            
            return accommodations;
        }

        //TO-DO: dodat metodu za filtriranje smjestaja po svemu gore + po check-in check-out datumima

        public async Task<Accommodation> GetAccommodationById(int accommodationId)
        {
            Accommodation accommodation;
            accommodation = await Context.Accommodations.Include("AccommodationType").Include("ApplicationUser")
                                                         .Where(accommodation =>
                                                         accommodation.Id == accommodationId)
                                                         .FirstOrDefaultAsync();

            return accommodation;
        }

        public async Task<List<string>> GetDatesOccupiedForAccommodation(int accommodationId)
        {
            List<string> listOfDatesOccupied = new List<string>();
            List<Booking> bookings;

            bookings = await Context.Bookings.Where(booking => booking.Accommodation.Id == accommodationId)
                            .OrderBy(booking => booking.CheckInDate)
                            .ToListAsync();

            foreach(Booking booking in bookings)
            {
                for(var currentDate = booking.CheckInDate; currentDate < booking.CheckOutDate; currentDate = currentDate.AddDays(1))
                {
                    listOfDatesOccupied.Add(currentDate.ToShortDateString());
                }
            }
            return listOfDatesOccupied;
        }
        
    }
}
