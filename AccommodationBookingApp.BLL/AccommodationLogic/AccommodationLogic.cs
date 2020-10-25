using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.AccommodationLogic
{
    public class AccommodationLogic
    {
        private IAccommodation _accommodation = new DataAccess.Functions.AccommodationFunctions();

        public async Task<Boolean> CreateNewAccomodation(Accommodation accommodation,
                                                         int accommodationTypeId,
                                                         string applicationUserId)
        {
            try
            {
                var result = await _accommodation.CreateAccommodation(accommodation,accommodationTypeId, applicationUserId);
                if(result.Id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception error)
            {
                return false;
            }
        }

        public async Task<List<Accommodation>> GetAccommodations()
        {
            List<Accommodation> accommodations = await _accommodation.GetAllAccommodations();

            return accommodations;
        }

        public async Task<List<Accommodation>> GetAccommodationsWithUserId(string userId)
        {
            List<Accommodation> accommodations = await _accommodation.GetAccommodationsWithUserIdAsync(userId);

            return accommodations;
        }

        public async Task<List<Accommodation>> GetFilteredAccommodations(string accommodationCity,
                             int accommodationTypeId, int numberOfGuests)
        {
            List<Accommodation> accommodations = await _accommodation.GetFilteredAccommodations(accommodationCity, accommodationTypeId, numberOfGuests);

            return accommodations;
        }
        
        public async Task<Accommodation> GetAccommodationById(int accommodationId)
        {
            return await _accommodation.GetAccommodationById(accommodationId);
        }
    }
}
