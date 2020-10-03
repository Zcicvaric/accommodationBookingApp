using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Functions
{
    public class UserFunctions : IUserTest
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserFunctions(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<bool> createNewUser(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            return result.Succeeded;
        }

        public async Task<bool> signInUser(ApplicationUser user, string password)
        {
            var userObject = await userManager.FindByEmailAsync(user.UserName);

            if (userObject != null)
            {
                var signInResult = await signInManager.PasswordSignInAsync(userObject, password, false, false);

                if (signInResult.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        
    }
}
