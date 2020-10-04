using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    public class AccommodationModel : PageModel
    {
        private AccommodationLogic AccommodationLogic;
        private UserManager<ApplicationUser> UserManager;
        public List<Accommodation> Accommodations { get; set; }
        public AccommodationModel(UserManager<ApplicationUser> userManager)
        {
            AccommodationLogic = new AccommodationLogic();
            UserManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await UserManager.GetUserAsync(User);
            Accommodations = await AccommodationLogic.GetAccommodationsWithUserId(user.Id);

            return Page();
        }
    }
}
