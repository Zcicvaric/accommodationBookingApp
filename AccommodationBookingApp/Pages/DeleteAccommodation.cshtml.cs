using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [Authorize(Roles = "Host, Admin")]
    public class DeleteAccommodationModel : PageModel
    {
        [BindProperty]
        public int AccommodationId { get; set; }
        public int NumberOfBookingsForAccommodation { get; set; }
        private IWebHostEnvironment WebHostEnvironment { get; }

        private readonly BookingLogic bookingLogic = new BookingLogic();
        private readonly AccommodationLogic accommodationLogic = new AccommodationLogic();

        public DeleteAccommodationModel(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync(int accommodationId)
        {
            AccommodationId = accommodationId;
            var accommodation = await accommodationLogic.GetAccommodationById(accommodationId);

            if (accommodation == null)
            {
                return NotFound();
            }

            if(accommodation.ApplicationUser.UserName != User.Identity.Name && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var allBookingsForAccommodation = await bookingLogic.GetAllUpcomingApprovedOrPendingReservationsForAccommodation(accommodationId);
            NumberOfBookingsForAccommodation = allBookingsForAccommodation.Count();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int accommodationId)
        {
            if (ModelState.IsValid)
            {
                var accommodationToBeDeleted = await accommodationLogic.GetAccommodationById(accommodationId);

                if(accommodationToBeDeleted.ApplicationUser.UserName != User.Identity.Name && !User.IsInRole("Admin"))
                {
                    return Unauthorized();
                }

                var accommodationPhotosFolder = Path.Combine(WebHostEnvironment.WebRootPath, "accommodationPhotos");

                var deleteSuccessful = await accommodationLogic.DeleteAccommodationAsync(accommodationToBeDeleted, accommodationPhotosFolder);

                if (!deleteSuccessful)
                {
                    return BadRequest();
                }

            }
            return RedirectToPage("/Accommodations");
        }
    }
}
