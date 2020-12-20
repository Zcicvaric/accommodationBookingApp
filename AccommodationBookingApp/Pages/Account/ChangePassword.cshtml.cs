using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        [BindProperty]
        [Required]
        [DataType (DataType.Password)]
        [Display (Name = "Old Password")]
        public string OldPassword { get; set; }
        [BindProperty]
        [Required]
        [DataType (DataType.Password)]
        [Display (Name = "New Password")]
        public string NewPassword { get; set; }
        [DataType (DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="Passwords must match!")]
        [Display (Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }

        public ChangePasswordModel(UserManager<ApplicationUser> userManager,
                                   SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                {
                    return BadRequest();
                }

                var passwordChangeResult = await userManager.ChangePasswordAsync(user, OldPassword, NewPassword);

                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var changePasswordError in passwordChangeResult.Errors)
                    {
                        ModelState.AddModelError("", changePasswordError.Description);
                    }

                    return Page();
                }

                //kreiramo novi pristupni token, tako da ako nam i napadac dode do naseg pristupnog tokena nece mu dugo vridit
                await signInManager.RefreshSignInAsync(user);

                return RedirectToPage("/Account/PasswordChanged", "PasswordChanged");
            }

            return BadRequest();
        }
    }
}
