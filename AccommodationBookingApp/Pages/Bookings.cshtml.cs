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
    public class BookingsModel : PageModel
    {
        private BookingLogic BookingLogic = new BookingLogic();
        private UserManager<ApplicationUser> UserManager;
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
            ApplicationUser applicationUser = await UserManager.GetUserAsync(User);
            PreviousStays = await BookingLogic.GetAllPreviousStaysForUser(applicationUser.Id);
            UpcomingStays = await BookingLogic.GetAllUpcomingStaysForUser(applicationUser.Id);
            DeclinedStays = await BookingLogic.GetAllDeclinedStaysForUser(applicationUser.Id);
            CancelledStays = await BookingLogic.GetAllCancelledStaysForUser(applicationUser.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostUserCancelBooking (int bookingId)
        {
            var result = await BookingLogic.CancelBookingAsUser(bookingId);

            if(result)
            {
                return RedirectToPage("/Bookings");
            }

            return Page();
        }
    }
}
