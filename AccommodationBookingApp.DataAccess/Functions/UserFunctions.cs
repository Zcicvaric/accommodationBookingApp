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
    public class UserFunctions : IUser
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserFunctions(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager,
                            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public UserFunctions(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<bool> CreateNewUser(ApplicationUser user, string password, bool registerAsHost)
        {
            var result = await userManager.CreateAsync(user, password);

            if(registerAsHost)
            {
                await userManager.AddToRoleAsync(user, "Host");
            }
            else
            {
                await userManager.AddToRoleAsync(user, "User");
            }

            await signInManager.PasswordSignInAsync(user, password, true, false);

            return result.Succeeded;
        }

        public async Task<bool> SignInUser(ApplicationUser user, string password)
        {
            var userObject = await userManager.FindByEmailAsync(user.UserName);

            if (userObject != null)
            {
                var signInResult = await signInManager.PasswordSignInAsync(userObject, password, true, false);

                if (signInResult.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> SignOutUser()
        {
            await signInManager.SignOutAsync();

            return true;
        }



    }
}
