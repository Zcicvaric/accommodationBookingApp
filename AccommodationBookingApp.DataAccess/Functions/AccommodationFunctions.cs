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
    public class AccommodationFunctions : IAccommodation
    {
        private readonly DatabaseContext Context;
        public AccommodationFunctions()
        {
            Context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions);
        }
        public async Task<Accommodation> CreateAccommodationAsync(Accommodation accommodation, int accommodationTypeId, int currencyId, string accommodationOwnerUsername)
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

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return new Accommodation();
            }

            return accommodation;
        }

        public async Task<bool> DeleteAccommodationAsync(Accommodation accommodationToDelete)
        {
            Context.Accommodations.Remove(accommodationToDelete);

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

        public async Task<List<Accommodation>> GetAllAccommodationsAsync()
        {
            var accommodations = await Context.Accommodations.Include("AccommodationType").Include("ApplicationUser")
                                   .ToListAsync();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetAccommodationsWithUserIdAsync(string userId)
        {
            var accommodations = await Context.Accommodations.Include("ApplicationUser").Include("Currency")
                                                         .Where(accommodation => accommodation.ApplicationUser.Id == userId)
                                                         .ToListAsync();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetFilteredAccommodationsAsync(string accommodationCity, int numberOfGuests)
        {
            var accommodations = await Context.Accommodations.Include("AccommodationType").Include("ApplicationUser")
                                                         .Include("Currency")
                                                         .Where(accommodation =>
                                                         accommodation.City == accommodationCity &&
                                                         accommodation.NumberOfBeds >= numberOfGuests)
                                                         .ToListAsync();

            return accommodations;
        }


        public async Task<Accommodation> GetAccommodationByIdAsync(int accommodationId)
        {
            var accommodation = await Context.Accommodations.Include("ApplicationUser").Include("Currency")
                                                         .Where(accommodation =>
                                                         accommodation.Id == accommodationId)
                                                         .FirstOrDefaultAsync();

            return accommodation;
        }

        public async Task<bool> UpdateAccommodationAsync(int accommodationId, string name, int numberOfBeds, int pricePerNight, int currencyId, bool requireApproval, bool userCanCancelBooking, string checkInTime, string checkOutTime)
        {
            var accommodationToBeUpdated = await Context.Accommodations.Where(accommodation => accommodation.Id == accommodationId)
                                                                       .FirstOrDefaultAsync();

            var currency = await Context.Currencies.Where(currency => currency.Id == currencyId).FirstOrDefaultAsync();

            if (accommodationToBeUpdated != null)
            {
                accommodationToBeUpdated.Name = name;
                accommodationToBeUpdated.NumberOfBeds = numberOfBeds;
                accommodationToBeUpdated.PricePerNight = pricePerNight;
                accommodationToBeUpdated.Currency = currency;
                accommodationToBeUpdated.UserCanCancelBooking = userCanCancelBooking;
                accommodationToBeUpdated.RequireApproval = requireApproval;
                accommodationToBeUpdated.CheckInTime = checkInTime;
                accommodationToBeUpdated.CheckOutTime = checkOutTime;
            }

            try
            {
                Context.Update(accommodationToBeUpdated);
                await Context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateAccommodationHeaderPhoto(int accommodationId, string newHeaderPhotoFileName)
        {
            var accommodationToBeUpdated = await Context.Accommodations.Where(accommodation => accommodation.Id == accommodationId).FirstOrDefaultAsync();

            if (accommodationToBeUpdated != null)
            {
                accommodationToBeUpdated.HeaderPhotoFileName = newHeaderPhotoFileName;
            }

            try
            {
                Context.Update(accommodationToBeUpdated);
                await Context.SaveChangesAsync();
            }
            catch
            {

                return false;
            }

            return true;
        }
    }
}
