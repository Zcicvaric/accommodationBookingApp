﻿using AccommodationBookingApp.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IAccommodation
    {
        Task<Accommodation> CreateAccommodationAsync(Accommodation accommodation, int accommodationTypeId, int currencyId, string accommodationOwnerUsername);
        Task<bool> UpdateAccommodationAsync(int accommodationId, string name, int numberOfBeds, int pricePerNight, int currencyId,
                                            bool requireApproval, bool userCanCancelBooking, string checkInTime, string checkOutTime);
        Task<bool> DeleteAccommodationAsync(Accommodation accommodationToDelete);
        Task<List<Accommodation>> GetAccommodationsWithUserIdAsync(string userId);
        Task<List<Accommodation>> GetAllAccommodationsAsync();
        Task<List<Accommodation>> GetFilteredAccommodationsAsync(string accommodationCity, int numberOfGuests);
        Task<Accommodation> GetAccommodationByIdAsync(int accommodationId);
        Task<bool> UpdateAccommodationHeaderPhoto(int accommodationId, string newHeaderPhotoFileName);
    }
}
