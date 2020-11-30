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

        UserManager<ApplicationUser> userManager;
       
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
        public UserLogic(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
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

        public async Task<IdentityResult> UpdateAccount(ApplicationUser oldUser, string firstName, string lastName, string email)
        {
            var userToUpdate = await userManager.FindByIdAsync(oldUser.Id);

            userToUpdate.FirstName = firstName;
            userToUpdate.LastName = lastName;
            userToUpdate.Email = email;
            userToUpdate.UserName = email;

            return await userManager.UpdateAsync(userToUpdate);
        }

        public async Task<IdentityResult> UpdateHostAccount(ApplicationUser oldHost, string firstName, string lastName, string email,
                                                            string address, string mobilePhoneNumber, string city, string country)
        {
            var hostToUpdate = await userManager.FindByIdAsync(oldHost.Id);

            hostToUpdate.FirstName = firstName;
            hostToUpdate.LastName = lastName;
            hostToUpdate.Email = email;
            hostToUpdate.UserName = email;
            hostToUpdate.Address = address;
            hostToUpdate.PhoneNumber = mobilePhoneNumber;
            hostToUpdate.City = city;
            hostToUpdate.Country = country;

            return await userManager.UpdateAsync(hostToUpdate);
        }


    }
}
