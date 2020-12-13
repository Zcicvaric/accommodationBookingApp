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
            this.userManager = userManager;
            _user = new DataAccess.Functions.UserFunctions(userManager, signInManager);
        }
        public UserLogic(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> CreateNewUser(string firstName, string lastName, 
                                                        string email, string password)
        {
            ApplicationUser newUser = new ApplicationUser();

            newUser.FirstName = firstName;
            newUser.LastName = lastName;
            newUser.Email = email;
            newUser.UserName = email;


            return await _user.CreateNewUser(newUser, password, false);
        }

        public async Task<IdentityResult> CreateNewHost(string firstName, string lastName, string email, string address, string city,
                                                        string country, string mobilePhoneNumber, string password)
        {
            ApplicationUser newHost = new ApplicationUser();

            newHost.FirstName = firstName;
            newHost.LastName = lastName;
            newHost.Email = email;
            newHost.UserName = email;
            newHost.Address = address;
            newHost.City = city;
            newHost.Country = country;
            newHost.PhoneNumber = mobilePhoneNumber;

            return await _user.CreateNewUser(newHost, password, true);
        }
        

        public async Task<bool> SignInUser(string username, string password, bool permanentCookie)
        {
            ApplicationUser user = await userManager.FindByNameAsync(username);
            
            if (user == null)
            {
                return false;
            }

            return await _user.SignInUser(user, password, permanentCookie);
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
