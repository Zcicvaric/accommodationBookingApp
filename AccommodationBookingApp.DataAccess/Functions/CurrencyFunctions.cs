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
    public class CurrencyFunctions : ICurrency
    {
        private DatabaseContext Context;

        public CurrencyFunctions()
        {
            Context = new DatabaseContext(DatabaseContext.optionsBuild.dbContextOptions);
        }

        public async Task<Currency> AddCurrency(Currency currency)
        {
            await Context.Currencies.AddAsync(currency);

            await Context.SaveChangesAsync();

            return currency;
        }

        public async Task<List<Currency>> GetAllCurrencies()
        {
            List<Currency> currencies = new List<Currency>();
            currencies = await Context.Currencies.ToListAsync();

            return currencies;
        }
    }
}
