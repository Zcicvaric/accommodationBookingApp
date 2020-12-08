using AccommodationBookingApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IAccommodationType
    {
        Task<AccommodationType> GetAccommodationTypeById(int accommodationTypeId);
        Task<List<AccommodationType>> GetAllAccommodationTypes();
    }
}
