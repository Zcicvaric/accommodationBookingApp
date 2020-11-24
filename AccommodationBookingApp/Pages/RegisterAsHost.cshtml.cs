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
    public class RegisterAsHostModel : PageModel
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
        [BindProperty]
        [Required]
        [Display(Name = "Mobile Phone Number")]
        [RegularExpression(@"^(\d{9,10})$", ErrorMessage = "Mobile phone number must have 9 or 10 digits")]
        public string MobilePhoneNumber { get; set; }

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
                ApplicationUser.PhoneNumber = MobilePhoneNumber;
                var result = await userLogic.CreateNewUser(ApplicationUser, Password, true);
                
                if(result.Succeeded)
                {
                    return RedirectToPage("/Index");
                }

                foreach(var registrationError in result.Errors)
                {
                    ModelState.AddModelError("", registrationError.Description);
                }
            }

            return Page();
        }
    }
}
