using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.AccommodationLogic
{
    public class AccommodationTypeLogic
    {

        private IAccommodationType _accommodationType = new DataAccess.Functions.AccommodationTypeFunctions();

        public async Task<List<AccommodationType>> GetAccommodationTypes()
        {
            List<AccommodationType> accommodationTypes = await _accommodationType.GetAllAccommodationTypes();

            return accommodationTypes;
        }
    }
}
