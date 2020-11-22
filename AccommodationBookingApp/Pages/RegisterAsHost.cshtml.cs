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
    public class RegisterAsHostModel : PageModel
    {
        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private UserLogic userLogic;

        public RegisterAsHostModel(UserManager<ApplicationUser> userManager,
                                   SignInManager<ApplicationUser> signInManager,
                                   RoleManager<IdentityRole> roleManager)
        {
            userLogic = new UserLogic(userManager, signInManager, roleManager);
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                ApplicationUser.UserName = ApplicationUser.Email;
                var successfulRegistration = await userLogic.CreateNewUser(ApplicationUser, Password, true);
                
                if(successfulRegistration)
                {
                    return RedirectToPage("/Index");
                }
            }

            return RedirectToPage("/Register");
        }
    }
}
