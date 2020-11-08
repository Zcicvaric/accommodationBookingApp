using AccommodationBookingApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IAccommodation
    {
        Task<Accommodation> CreateAccommodation(Accommodation accommodation,
                                                int accommodationTypeId,
                                                string applicationUserId);
        Task<List<Accommodation>> GetAccommodationsWithUserIdAsync(string userId);
        Task<List<Accommodation>> GetAllAccommodations();
        Task<List<Accommodation>> GetFilteredAccommodations(string accommodationCity, int accommodationTypeId, int numberOfGuests,
                                                            DateTime checkInDate, DateTime checkOutDate);
        Task<Accommodation> GetAccommodationById(int accommodationId);
        Task<List<String>> GetDatesOccupiedForAccommodation(int accommodationId);
    }
}
