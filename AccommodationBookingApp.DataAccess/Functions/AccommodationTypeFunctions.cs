using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Functions
{
    public class AccommodationTypeFunctions : IAccommodationType
    {
        public async Task<List<AccommodationType>> GetAllAccommodationTypes()
        {
            List<AccommodationType> accommodationTypes = new List<AccommodationType>();

            using (var context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions))
            {
                accommodationTypes = await context.AccommodationType.ToListAsync();
            }
            return accommodationTypes;
        }
    }
}
