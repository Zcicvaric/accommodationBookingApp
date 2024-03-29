﻿using AccommodationBookingApp.DataAccess.Entities;
using AccommodationBookingApp.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.BLL.UserLogic
{
    public class UserLogic
    {
        private readonly IUser userFunctions;
        private readonly UserManager<ApplicationUser> userManager;

        public UserLogic(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager,
                        RoleManager<IdentityRole> roleManager)
        {
            userFunctions = new DataAccess.Functions.UserFunctions(userManager, signInManager, roleManager);
        }
        public UserLogic(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            userFunctions = new DataAccess.Functions.UserFunctions(userManager, signInManager);
        }
        public UserLogic(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            userFunctions = new DataAccess.Functions.UserFunctions(userManager);
        }

        public async Task<IdentityResult> CreateNewUserAsync(string firstName, string lastName,
                                                        string email, string password)
        {
            var newUser = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email
            };


            return await userFunctions.CreateNewUserAsync(newUser, password, false);
        }

        public async Task<IdentityResult> CreateNewHostAsync(string firstName, string lastName, string email, string address, string city,
                                                        string country, string mobilePhoneNumber, string password)
        {
            var newHost = new ApplicationUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                Address = address,
                City = city,
                Country = country,
                PhoneNumber = mobilePhoneNumber
            };

            return await userFunctions.CreateNewUserAsync(newHost, password, true);
        }


        public async Task<bool> SignInUserAsync(string username, string password, bool permanentCookie)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                return false;
            }

            return await userFunctions.SignInUserAsync(user, password, permanentCookie);
        }

        public async Task<bool> SignOutUserAsync()
        {
            return await userFunctions.SignOutUserAsync();
        }

        public async Task<IdentityResult> UpdateAccountAsync(ApplicationUser oldUser, string firstName, string lastName, string email)
        {
            var userToUpdate = await userManager.FindByIdAsync(oldUser.Id);

            userToUpdate.FirstName = firstName;
            userToUpdate.LastName = lastName;
            userToUpdate.Email = email;
            userToUpdate.UserName = email;

            return await userManager.UpdateAsync(userToUpdate);
        }

        public async Task<IdentityResult> UpdateHostAccountAsync(ApplicationUser oldHost, string firstName, string lastName, string email,
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

        public async Task<List<ApplicationUser>> GetAllUserAccountsAsync()
        {
            return await userFunctions.GetAllUserAccountsAsync();
        }

        public async Task<List<ApplicationUser>> GetAllHostAccountsAsync()
        {
            return await userFunctions.GetAllHostAccountsAsync();
        }


    }
}
