using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.CurrencyLogic
{
    public class CurrencyLogic
    {
        private readonly ICurrency currencyFunctions = new DataAccess.Functions.CurrencyFunctions();

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            var currencies = await currencyFunctions.GetAllCurrenciesAsync();

            return currencies;
        }

        public async Task<bool> AddCurrencyAsync(string name)
        {
            var allCurrencies = await currencyFunctions.GetAllCurrenciesAsync();

            var currenciesWithName = allCurrencies.Where(currency => currency.Name == name).ToList();

            if (currenciesWithName.Count != 0)
            {
                return false;
            }

            var newCurrency = new Currency
            {
                Name = name
            };

            try
            {
                var result = await currencyFunctions.CreateCurrencyAsync(newCurrency);

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
