using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages
{
    [Authorize(Roles = "Host, Admin")]
    public class AccommodationsModel : PageModel
    {
        private readonly AccommodationLogic AccommodationLogic;
        private readonly UserManager<ApplicationUser> UserManager;
        public List<Accommodation> Accommodations { get; set; }
        public string HeaderImageFolderPath { get; set; }
        private IWebHostEnvironment WebHostEnvironment { get; }
        public string HostName { get; set; }

        public AccommodationsModel(UserManager<ApplicationUser> userManager,
                                  IWebHostEnvironment webHostEnvironment)
        {
            AccommodationLogic = new AccommodationLogic();
            UserManager = userManager;
            WebHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await UserManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Login");
            }

            if (User.IsInRole("Admin"))
            {
                return NotFound();
            }

            Accommodations = await AccommodationLogic.GetAccommodationsWithUserIdAsync(user.Id);

            return Page();
        }

        public async Task<IActionResult> OnGetAccommodationsForHostAsync(string username)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            if (username == null)
            {
                return BadRequest();
            }

            var user = await UserManager.FindByNameAsync(username);

            HostName = user.FirstName + " " + user.LastName;

            Accommodations = await AccommodationLogic.GetAccommodationsWithUserIdAsync(user.Id);

            return Page();
        }
    }
}
