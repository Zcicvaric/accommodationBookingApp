using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    public class LoginModel : PageModel
    {

        [BindProperty]
        public ApplicationUser User { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private UserLogic userLogic;

        public LoginModel(UserManager<ApplicationUser> userManager,
                          SignInManager<ApplicationUser> signInManager)
        {
            this.userLogic = new UserLogic(userManager, signInManager);
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            bool result = await userLogic.SignInUser(User, Password);

            if (result)
            {
                return RedirectToPage("/index");
            }
            return RedirectToPage("/login");
        }
    }
}