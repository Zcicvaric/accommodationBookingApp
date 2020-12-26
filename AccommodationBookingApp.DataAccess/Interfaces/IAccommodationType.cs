using AccommodationBookingApp.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IAccommodationType
    {
        Task<AccommodationType> GetAccommodationTypeByIdAsync(int accommodationTypeId);
        Task<List<AccommodationType>> GetAllAccommodationTypesAsync();
        Task<AccommodationType> CreateAccommodationTypeAsync(AccommodationType accommodationType);
    }
}
