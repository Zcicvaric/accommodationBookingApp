using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Functions
{
    public class CurrencyFunctions : ICurrency
    {
        private readonly DatabaseContext Context;

        public CurrencyFunctions()
        {
            Context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions);
        }

        public async Task<Currency> CreateCurrencyAsync(Currency currency)
        {
            await Context.Currencies.AddAsync(currency);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {

                return new Currency();
            }

            return currency;
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            var currencies = await Context.Currencies.ToListAsync();

            return currencies;
        }
    }
}
