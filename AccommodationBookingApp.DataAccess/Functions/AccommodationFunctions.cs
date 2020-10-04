using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Functions
{
    public class AccommodationFunctions : IAccommodation
    {
        public async Task<Accommodation> CreateAccommodation(Accommodation accommodation)
        {
            using (var context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions))
            {
                await context.Accommodations.AddAsync(accommodation);
                await context.SaveChangesAsync();
            }

            return accommodation;
        }

        public async Task<List<Accommodation>> GetAllAccommodations()
        {
            List<Accommodation> accommodations = new List<Accommodation>();

            using (var context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions))
            {
                accommodations = await context.Accommodations.ToListAsync();
            }

            return accommodations;
        }

        public async Task<List<Accommodation>> GetAccommodationsWithUserIdAsync(string userId)
        {
            List<Accommodation> accommodations;

            using (var context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions))
            {
                accommodations = await context.Accommodations.Where(accommodation => 
                                                              accommodation.ApplicationUserId == userId).ToListAsync();
            }

            return accommodations;
        }
    }
}
