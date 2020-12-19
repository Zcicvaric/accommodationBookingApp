using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [Authorize (Roles = "User")]
    [BindProperties]
    public class UpdateAccountModel : PageModel
    {
        private UserLogic userLogic;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        [BindNever]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display (Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display (Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [DataType (DataType.EmailAddress)]
        public string Email { get; set; }

        public UpdateAccountModel(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager)
        {
            this.userLogic = new UserLogic(userManager);
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task<IActionResult> OnGet()
        {
            ApplicationUser = await userManager.GetUserAsync(User);
            
            return Page();
        }


        //Can't update the user here using the userManager's update method due to the optimistic concurrency failure
        //(caused when multiple users try to change the same object
        public async Task<IActionResult> OnPost()
        {
            ApplicationUser = await userManager.GetUserAsync(User);

            if (ApplicationUser == null)
            {
                return RedirectToPage("/UpdateAccount");
            }

            if (ModelState.IsValid)
            {
                var userUpdateResult = await userLogic.UpdateAccount(ApplicationUser, FirstName, LastName, Email);
                
                if(userUpdateResult.Succeeded)
                {
                    await signInManager.RefreshSignInAsync(ApplicationUser);
                    return RedirectToPage("/Account");
                }

                foreach (var userUpdateError in userUpdateResult.Errors)
                {
                    ModelState.AddModelError("", userUpdateError.Description);
                }
                return Page();
            }

            return Page();
        }
    }
}
