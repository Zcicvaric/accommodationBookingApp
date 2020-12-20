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
        private readonly DatabaseContext Context;
        public AccommodationFunctions()
        {
            Context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions);
        }
        public async Task<Accommodation> CreateAccommodation(Accommodation accommodation, int accommodationTypeId, int currencyId, string accommodationOwnerUsername)
        {
            var accommodationType = await Context.AccommodationType.Where(accommodationType =>
                                                                                  accommodationType.Id == accommodationTypeId)
                                                                                  .FirstOrDefaultAsync();

            var currency = await Context.Currencies.Where(currency => currency.Id == currencyId)
                                                        .FirstOrDefaultAsync();

            var applicationUser = await Context.Users.Where(user =>
                                                            user.UserName == accommodationOwnerUsername)
                                                            .FirstOrDefaultAsync();

            accommodation.AccommodationType = accommodationType;
            accommodation.Currency = currency;
            accommodation.ApplicationUser = applicationUser;

            await Context.Accommodations.AddAsync(accommodation);
            await Context.SaveChangesAsync();

            return accommodation;
        }

        public async Task<bool> DeleteAccommodationAsync(Accommodation accommodationToDelete)
        {
            Context.Accommodations.Remove(accommodationToDelete);

            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Accommodation>> GetAllAccommodations()
        {
            var accommodations = await Context.Accommodations.Include("AccommodationType").Include("ApplicationUser")
                                   .ToListAsync();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetAccommodationsWithUserIdAsync(string userId)
        {
            var accommodations = await Context.Accommodations.Include("AccommodationType").Include("ApplicationUser")
                                                         .Include("Currency")
                                                         .Where(accommodation => accommodation.ApplicationUser.Id == userId).ToListAsync();

            return accommodations;
        }

        //mos na ovo napravit prezentaciju kako radi 
        public async Task<List<Accommodation>> GetFilteredAccommodations(string accommodationCity, int numberOfGuests)
        {
            var accommodations = await Context.Accommodations.Include("AccommodationType").Include("ApplicationUser")
                                                         .Include("Currency")
                                                         .Where(accommodation =>
                                                         accommodation.City == accommodationCity &&
                                                         accommodation.NumberOfBeds >= numberOfGuests).ToListAsync();
            
            return accommodations;
        }


        public async Task<Accommodation> GetAccommodationById(int accommodationId)
        {
            var accommodation = await Context.Accommodations.Include("AccommodationType").Include("ApplicationUser")
                                                         .Include("Currency")
                                                         .Where(accommodation =>
                                                         accommodation.Id == accommodationId)
                                                         .FirstOrDefaultAsync();

            return accommodation;
        }
        
    }
}
