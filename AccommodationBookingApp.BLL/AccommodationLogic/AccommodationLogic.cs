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

        public async Task<Boolean> CreateNewAccomodation(Accommodation accommodation)
        {
            try
            {
                var result = await _accommodation.CreateAccommodation(accommodation);
                if(result.AccommodationId > 0)
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
    }
}
