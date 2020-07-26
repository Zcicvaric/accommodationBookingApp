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
        private IUserTest _user = new DataAccess.Functions.UserFunctions();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;


        public async Task<bool> CreateNewUser(ApplicationUser user, string password)
        {
           return await _user.createNewUser(user, password);
        }


    }
}
