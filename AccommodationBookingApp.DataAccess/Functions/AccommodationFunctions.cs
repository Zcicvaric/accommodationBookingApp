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
    public class AccommodationFunctions : IAccommodation
    {
        private DatabaseContext Context;
        public AccommodationFunctions()
        {
            Context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions);
        }
        public async Task<Accommodation> CreateAccommodation(Accommodation accommodation)
        {
            await Context.Accommodations.AddAsync(accommodation);
            await Context.SaveChangesAsync();

            return accommodation;
        }

        public async Task<List<Accommodation>> GetAllAccommodations()
        {
            List<Accommodation> accommodations = new List<Accommodation>();
            accommodations = await Context.Accommodations.ToListAsync();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetAccommodationsWithUserIdAsync(string userId)
        {
            List<Accommodation> accommodations;
            accommodations = await Context.Accommodations.Where(accommodation => 
                                                              accommodation.ApplicationUserId == userId).ToListAsync();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetFilteredAccommodations(string accommodationCity, int accommodationTypeId, int numberOfGuests)
        {
            List<Accommodation> accommodations;
            accommodations = await Context.Accommodations.Where(accommodation =>
                                                                accommodation.City == accommodationCity &&
                                                                accommodation.AccommodationTypeId == accommodationTypeId
                                                                && accommodation.NumberOfBeds >= numberOfGuests).ToListAsync();

            return accommodations;
        }

        //TO-DO: dodat metodu za filtriranje smjestaja po svemu gore + po check-in check-out datumima

        public async Task<Accommodation> GetAccommodationById(int accommodationId)
        {
            Accommodation accommodation;
            accommodation = await Context.Accommodations.Where(accommodation =>
                                                         accommodation.Id == accommodationId)
                                                         .FirstOrDefaultAsync();

            return accommodation;
        }
    }
}
