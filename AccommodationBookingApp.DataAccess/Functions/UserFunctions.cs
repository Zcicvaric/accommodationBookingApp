using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationBookingApp.DataAccess.Functions
{
    public class UserFunctions : IUserTest
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserFunctions(UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager):this()
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public UserFunctions()
        {
            
        }

        public async Task<bool> createNewUser(ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            return result.Succeeded;
        }

        
    }
}
