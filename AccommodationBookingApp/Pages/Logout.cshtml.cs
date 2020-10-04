using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    public class LogoutModel : PageModel
    {
        private UserLogic userLogic;

        public LogoutModel(UserManager<ApplicationUser> userManager,
                          SignInManager<ApplicationUser> signInManager)
        {
            userLogic = new UserLogic(userManager, signInManager);
        }
        public async Task<IActionResult> OnGet()
        {
            await userLogic.SignOutUser();

            return RedirectToPage("/Index");
        }
    }
}
