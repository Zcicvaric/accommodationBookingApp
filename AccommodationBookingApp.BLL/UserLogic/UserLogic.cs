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
        private IUser _user;

        public UserLogic(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager,
                        RoleManager<IdentityRole> roleManager)
        {
            _user = new DataAccess.Functions.UserFunctions(userManager, signInManager, roleManager);
        }
        public UserLogic(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager)
        {
            _user = new DataAccess.Functions.UserFunctions(userManager, signInManager);
        }

        public async Task<IdentityResult> CreateNewUser(ApplicationUser user, string password, bool registerAsHost)
        {
           return await _user.CreateNewUser(user, password, registerAsHost);
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
