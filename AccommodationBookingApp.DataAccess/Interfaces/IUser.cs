using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Interfaces
{
    public interface IUser
    {
        Task<IdentityResult> CreateNewUser(ApplicationUser user, string password, bool registerAsHost);
        Task<bool> SignInUser(ApplicationUser user, string password);
        Task<bool> SignOutUser();
    }
}
