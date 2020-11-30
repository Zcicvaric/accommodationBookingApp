using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        private UserLogic userLogic;

        public LoginModel(UserManager<ApplicationUser> userManager,
                          SignInManager<ApplicationUser> signInManager)
        {
            this.userLogic = new UserLogic(userManager, signInManager);
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(string ReturnUrl)
        {
            bool result = await userLogic.SignInUser(User, Password);

            if (!result)
            {
                ErrorMessage = "Invalid email and password combination!";
                return Page();
            }

            //redirect only to local pages in order to prevent open redirect vulnerability to be exploited
            if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToPage("/Index");
        }
    }
}