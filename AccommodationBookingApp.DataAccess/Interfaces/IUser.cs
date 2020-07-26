using AccommodationBookingApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IUserTest
    {
        Task<bool> createNewUser(ApplicationUser user, string password);
    }
}
