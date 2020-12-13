using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public async Task<Boolean> AddAccommodationType(string name)
        {
            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);

            var allAccommodationTypes = await _accommodationType.GetAllAccommodationTypes();

            var accommodationTypeWithName = allAccommodationTypes.Where(accommodationType => accommodationType.Name == name)
                                            .ToList();

            if(accommodationTypeWithName.Count != 0)
            {
                return false;
            }

            AccommodationType newAccommodationType = new AccommodationType();
            newAccommodationType.Name = name;

            try
            {
                var result = await _accommodationType.AddAccommodationType(newAccommodationType);

                if (result.Id != 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }
    }
}
