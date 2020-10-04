using AccommodationBookingApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IUserTest
    {
        Task<bool> CreateNewUser(ApplicationUser user, string password);
        Task<bool> SignInUser(ApplicationUser user, string password);
        Task<bool> SignOutUser();
    }
}
