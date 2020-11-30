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
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [Authorize (Roles = "User")]
    public class UpdateAccountModel : PageModel
    {
        //dodat ovde sve property i onda njih dodat u current user objekt
        private UserLogic userLogic;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [BindProperty]
        [Display (Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [BindProperty]
        [Display (Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [BindProperty]
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


        //u ovom slucaju se nije moglo odmah update usera u controlleru radi optimistic concurrency failure (na dva mista se odjednom
        //pristupa objektu
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
