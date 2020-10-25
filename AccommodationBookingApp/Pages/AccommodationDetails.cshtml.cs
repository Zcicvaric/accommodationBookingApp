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
    public class AccommodationDetailsModel : PageModel
    {
        [BindProperty]
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public string ImagePath { get; set; }
        public int MyProperty { get; set; }
        public ApplicationUser CurrentUser { get; set; }

        private AccommodationLogic AccommodationLogic = new AccommodationLogic();
        private readonly UserManager<ApplicationUser> userManager;
        private BookingLogic BookingLogic = new BookingLogic();


        public AccommodationDetailsModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int accommodationId)
        {
            AccommodationId = accommodationId;
            Accommodation = await AccommodationLogic.GetAccommodationById(accommodationId);
            CurrentUser = await userManager.GetUserAsync(User);


            ImagePath = "~/accommodationImages/" + Accommodation.Name + "/" + Accommodation.HeaderPhotoFileName;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            CurrentUser = await userManager.GetUserAsync(User);
            await BookingLogic.CreateNewBooking(AccommodationId, CurrentUser.Id);
            return RedirectToPage("/Bookings");
        }
    }
}
