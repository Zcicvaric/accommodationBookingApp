using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [BindProperties]
    public class RegisterModel : PageModel
    {
        [Required]
        [Display (Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display (Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType (DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [BindNever]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirm")]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        public string ConfirmPassword { get; set; }

        private readonly UserLogic userLogic;

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
                var result = await userLogic.CreateNewUser(FirstName, LastName, Email, Password);

                if(result.Succeeded)
                {
                     return RedirectToPage("/index");
                }
                foreach (var registrationError in result.Errors)
                {
                    ModelState.AddModelError("", registrationError.Description);

                    return Page();
                }
                 
            }

            return BadRequest();
        }
    }
}