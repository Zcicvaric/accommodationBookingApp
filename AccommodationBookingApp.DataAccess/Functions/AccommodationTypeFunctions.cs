using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Functions
{
    public class AccommodationTypeFunctions : IAccommodationType
    {
        private readonly DatabaseContext Context;
        public AccommodationTypeFunctions()
        {
            Context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions);
        }
        public async Task<AccommodationType> GetAccommodationTypeByIdAsync(int accommodationTypeId)
        {
            var accommodationType = await Context.AccommodationType.Where(accommodationType =>
                                                  accommodationType.Id == accommodationTypeId).FirstOrDefaultAsync();

            return accommodationType;
        }
        public async Task<List<AccommodationType>> GetAllAccommodationTypesAsync()
        {
            var accommodationTypes = await Context.AccommodationType.ToListAsync();

            return accommodationTypes;
        }

        public async Task<AccommodationType> CreateAccommodationTypeAsync(AccommodationType accommodationType)
        {
            await Context.AccommodationType.AddAsync(accommodationType);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return new AccommodationType();
            }

            return accommodationType;
        }
    }
}
