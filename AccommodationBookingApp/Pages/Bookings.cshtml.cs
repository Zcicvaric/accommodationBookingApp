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
    [Authorize]
    public class BookingsModel : PageModel
    {
        private readonly BookingLogic BookingLogic = new BookingLogic();
        private readonly UserManager<ApplicationUser> UserManager;
        private ApplicationUser ApplicationUser;
        public List<Booking> PreviousStays { get; set; }
        public List<Booking> UpcomingStays { get; set; }
        public List<Booking> DeclinedStays { get; set; }
        public List<Booking> CancelledStays { get; set; }
        public string Username { get; set; }

        public BookingsModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("Admin"))
            {
                return BadRequest();
            }

            ApplicationUser = await UserManager.GetUserAsync(User);

            if (ApplicationUser == null)
            {
                return RedirectToPage("/Login");
            }

            PreviousStays = await BookingLogic.GetAllPreviousStaysForUser(ApplicationUser.Id);
            UpcomingStays = await BookingLogic.GetAllUpcomingStaysForUser(ApplicationUser.Id);
            DeclinedStays = await BookingLogic.GetAllDeclinedStaysForUser(ApplicationUser.Id);
            CancelledStays = await BookingLogic.GetAllCancelledStaysForUser(ApplicationUser.Id);

            return Page();
        }

        public async Task<IActionResult> OnGetUserBookingsAsync(string username)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            ApplicationUser = await UserManager.FindByNameAsync(username);

            if (ApplicationUser == null)
            {
                return BadRequest();
            }

            Username = ApplicationUser.FirstName + " " + ApplicationUser.LastName;

            PreviousStays = await BookingLogic.GetAllPreviousStaysForUser(ApplicationUser.Id);
            UpcomingStays = await BookingLogic.GetAllUpcomingStaysForUser(ApplicationUser.Id);
            DeclinedStays = await BookingLogic.GetAllDeclinedStaysForUser(ApplicationUser.Id);
            CancelledStays = await BookingLogic.GetAllCancelledStaysForUser(ApplicationUser.Id);


            return Page();
        }

    }
}
