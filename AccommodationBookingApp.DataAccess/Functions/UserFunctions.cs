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
using AccommodationBookingApp.DataAccess.DataContext;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        public UserFunctions(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> CreateNewUser(ApplicationUser newUser, string password, bool registerAsHost)
        {
      
            var result = await userManager.CreateAsync(newUser, password);

            if (result.Succeeded)
            {
                if (registerAsHost)
            {
                await userManager.AddToRoleAsync(newUser, "Host");
            }
            else
            {
                await userManager.AddToRoleAsync(newUser, "User");
            }

            await signInManager.PasswordSignInAsync(newUser, password, true, false);
            }

            return result;
        }

        public async Task<bool> SignInUser(ApplicationUser user, string password, bool permanentCookie)
        {
            var userObject = await userManager.FindByEmailAsync(user.Email);

            if (userObject != null)
            {
                var signInResult = await signInManager.PasswordSignInAsync(userObject, password, permanentCookie, false);

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

        public async Task<List<ApplicationUser>> GetAllUserAccountsAsync()
        {
            var users = await userManager.GetUsersInRoleAsync("User");

            return (List<ApplicationUser>)users;
        }

        public async Task<List<ApplicationUser>> GetAllHostAccountsAsync()
        {
            var hosts = await userManager.GetUsersInRoleAsync("Host");

            return (List<ApplicationUser>)hosts;
        }


    }
}
