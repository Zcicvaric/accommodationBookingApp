using System;
using System.Collections.Generic;
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
    [Authorize]
    public class AccountModel : PageModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        private readonly UserManager<ApplicationUser> userManager;
        public string UserRole { get; set; }

        public AccountModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            ApplicationUser = await userManager.GetUserAsync(User);

            if (ApplicationUser == null)
            {
                return RedirectToPage("/Login");
            }

            var roles = await userManager.GetRolesAsync(ApplicationUser);

            UserRole = roles.FirstOrDefault();

            return Page();
        }

        public async Task<IActionResult> OnGetAccountDetailsForUserAsync(string username)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            ApplicationUser = await userManager.FindByNameAsync(username);

            if (ApplicationUser == null)
            {
                return BadRequest();
            }

            var roles = await userManager.GetRolesAsync(ApplicationUser);

            UserRole = roles.FirstOrDefault();

            return Page();
        }
    }
}
