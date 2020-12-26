using AccommodationBookingApp.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface ICurrency
    {
        Task<List<Currency>> GetAllCurrenciesAsync();
        Task<Currency> CreateCurrencyAsync(Currency currency);
    }
}
