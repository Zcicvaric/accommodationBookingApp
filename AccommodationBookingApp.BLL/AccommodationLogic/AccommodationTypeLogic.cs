using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.AccommodationLogic
{
    public class AccommodationTypeLogic
    {

        private readonly IAccommodationType accommodationTypeFunctions = new DataAccess.Functions.AccommodationTypeFunctions();

        public async Task<List<AccommodationType>> GetAccommodationTypesAsync()
        {
            var accommodationTypes = await accommodationTypeFunctions.GetAllAccommodationTypesAsync();

            return accommodationTypes;
        }

        public async Task<bool> AddAccommodationTypeAsync(string name)
        {
            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);

            var allAccommodationTypes = await accommodationTypeFunctions.GetAllAccommodationTypesAsync();

            var accommodationTypeWithName = allAccommodationTypes.Where(accommodationType => accommodationType.Name == name)
                                            .ToList();

            if (accommodationTypeWithName.Count != 0)
            {
                return false;
            }

            var newAccommodationType = new AccommodationType();
            newAccommodationType.Name = name;

            try
            {
                var result = await accommodationTypeFunctions.CreateAccommodationTypeAsync(newAccommodationType);

                if (result.Id != 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}
