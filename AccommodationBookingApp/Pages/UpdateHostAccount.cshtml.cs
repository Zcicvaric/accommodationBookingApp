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
    [Authorize (Roles = "Host")]
    [BindProperties]
    public class UpdateHostAccountModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        private UserLogic userLogic;

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
        [Required]
        public string Address { get; set; }
        [Required]
        [Display (Name = "Mobile Phone Number")]
        [RegularExpression(@"^(\d{9,10})$", ErrorMessage = "Mobile phone numbe must have 9 or 10 digits")]
        public string MobilePhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public UpdateHostAccountModel(UserManager<ApplicationUser> userManager,
                                      SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

            userLogic = new UserLogic(userManager);
        }
        public async Task<IActionResult> OnGet()
        {
            ApplicationUser = await userManager.GetUserAsync(User);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            ApplicationUser = await userManager.GetUserAsync(User);

            if (ApplicationUser == null)
            {
                return RedirectToPage("/UpdateHostAccount");
            }

            if (ModelState.IsValid)
            {
                var hostUpdateResult = await userLogic.UpdateHostAccount(ApplicationUser, FirstName, LastName, Email, Address,
                                                                         MobilePhoneNumber, City, Country);

                if (hostUpdateResult.Succeeded)
                {
                    await signInManager.RefreshSignInAsync(ApplicationUser);
                    return RedirectToPage("/Account");
                }

                foreach (var error in hostUpdateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Page();
            }

            return Page();
        }
    }
}
