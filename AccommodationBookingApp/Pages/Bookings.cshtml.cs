using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [Authorize (Roles = "User, Host")]
    public class BookingsModel : PageModel
    {
        private BookingLogic BookingLogic = new BookingLogic();
        private UserManager<ApplicationUser> UserManager;
        private ApplicationUser ApplicationUser;
        public List<Booking> PreviousStays { get; set; }
        public List<Booking> UpcomingStays { get; set; }
        public List<Booking> DeclinedStays { get; set; }
        public List<Booking> CancelledStays { get; set; }

        public BookingsModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await UserManager.GetUserAsync(User);
            PreviousStays = await BookingLogic.GetAllPreviousStaysForUser(ApplicationUser.Id);
            UpcomingStays = await BookingLogic.GetAllUpcomingStaysForUser(ApplicationUser.Id);
            DeclinedStays = await BookingLogic.GetAllDeclinedStaysForUser(ApplicationUser.Id);
            CancelledStays = await BookingLogic.GetAllCancelledStaysForUser(ApplicationUser.Id);

            return Page();
        }

    }
}
