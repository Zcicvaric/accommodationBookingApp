using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.CurrencyLogic
{
    public class CurrencyLogic
    {
        private ICurrency _currency = new DataAccess.Functions.CurrencyFunctions();

        public async Task<List<Currency>> GetCurrenciesAsync()
        {
            List<Currency> currencies = await _currency.GetAllCurrencies();

            return currencies;
        }

        public async Task<Boolean> AddCurrencyAsync(string name)
        {
            var allCurrencies = await _currency.GetAllCurrencies();

            var currenciesWithName = allCurrencies.Where(currency => currency.Name == name).ToList();

            if (currenciesWithName.Count != 0)
            {
                return false;
            }

            Currency newCurrency = new Currency();
            newCurrency.Name = name;

            try
            {
                var result = await _currency.AddCurrency(newCurrency);

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
