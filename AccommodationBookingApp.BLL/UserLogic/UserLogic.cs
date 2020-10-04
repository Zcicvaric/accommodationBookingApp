using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AccommodationBookingApp.BLL.UserLogic
{
    public class UserLogic
    {
        private IUserTest _user;

        public UserLogic(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager)
        {
            _user = new DataAccess.Functions.UserFunctions(userManager, signInManager);
        }

        public async Task<bool> CreateNewUser(ApplicationUser user, string password)
        {
           return await _user.CreateNewUser(user, password);
        }

        public async Task<bool> SignInUser(ApplicationUser user, string password)
        {
            return await _user.SignInUser(user, password);
        }

        public async Task<bool> SignOutUser()
        {
            return await _user.SignOutUser();
        }


    }
}
