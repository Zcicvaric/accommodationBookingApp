using AccommodationBookingApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface ICurrency
    {
        Task<List<Currency>> GetAllCurrencies();
        Task<Currency> AddCurrency(Currency currency);
    }
}
