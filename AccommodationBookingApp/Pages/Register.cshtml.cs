using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    public class RegisterModel : PageModel
    {
        
        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        public string ConfirmPassword { get; set; }

        private UserLogic userLogic;

        public RegisterModel(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager,
                            RoleManager<IdentityRole> roleManager)
        {
            this.userLogic = new UserLogic(userManager, signInManager, roleManager);
        }

        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
                ApplicationUser.UserName = ApplicationUser.Email;
                var result = await userLogic.CreateNewUser(ApplicationUser, Password, false);

                if(result.Succeeded)
                {
                     return RedirectToPage("/index");
                }
                foreach (var registrationError in result.Errors)
                {
                    ModelState.AddModelError("", registrationError.Description);
                }
                 
            }

            return Page();
            
        }
    }
}