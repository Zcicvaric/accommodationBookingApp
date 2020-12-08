using AccommodationBookingApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IAccommodation
    {
        Task<Accommodation> CreateAccommodation(Accommodation accommodation, int accommodationTypeId, string accommodationOwnerUsername);
        Task<List<Accommodation>> GetAccommodationsWithUserIdAsync(string userId);
        Task<List<Accommodation>> GetAllAccommodations();
        Task<List<Accommodation>> GetFilteredAccommodations(string accommodationCity, int numberOfGuests);
        Task<Accommodation> GetAccommodationById(int accommodationId);
    }
}
