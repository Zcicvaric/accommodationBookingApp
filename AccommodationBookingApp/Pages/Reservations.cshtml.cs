using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages
{
    [Authorize(Roles = "Host, Admin")]
    public class ReservationsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly BookingLogic BookingLogic = new BookingLogic();

        public List<Booking> PendingReservations { get; set; }
        public List<Booking> ApprovedReservations { get; set; }
        public List<Booking> PreviousReservations { get; set; }
        public List<Booking> DeclinedReservations { get; set; }
        public List<Booking> CancelledReservations { get; set; }
        public List<Booking> CancelledByUserReservations { get; set; }
        private ApplicationUser CurrentUser { get; set; }

        public ReservationsModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await UserManager.GetUserAsync(User);

            if (User == null)
            {
                return RedirectToPage("/Login");
            }

            PendingReservations = await BookingLogic.GetAllPendingReservationsForHostAsync(CurrentUser.Id);
            ApprovedReservations = await BookingLogic.GetAllApprovedReservationsForHostAsync(CurrentUser.Id);
            PreviousReservations = await BookingLogic.GetAllPreviousReservationsForHostAsync(CurrentUser.Id);
            DeclinedReservations = await BookingLogic.GetAllDeclinedReservationsForHostAsync(CurrentUser.Id);
            CancelledReservations = await BookingLogic.GetAllCancelledReservationsForHostAsync(CurrentUser.Id);
            CancelledByUserReservations = await BookingLogic.GetAllCancelledByUserReservationsForHostAsync(CurrentUser.Id);


            return Page();
        }

        public async Task<IActionResult> OnGetForAccommodationAsync(int accommodationId)
        {
            if (accommodationId == 0)
            {
                return BadRequest();
            }

            var accommodationLogic = new AccommodationLogic();
            var accommodation = await accommodationLogic.GetAccommodationByIdAsync(accommodationId);

            CurrentUser = await UserManager.GetUserAsync(User);

            if (CurrentUser == null)
            {
                return BadRequest();
            }

            if (accommodation.ApplicationUser.Id != CurrentUser.Id && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            PendingReservations = await BookingLogic.GetAllPendingReservationsForAccommodationAsync(accommodationId);
            ApprovedReservations = await BookingLogic.GetAllApprovedReservationsForAccommodationAsync(accommodationId);
            PreviousReservations = await BookingLogic.GetAllPreviousReservationsForAccommodationAsync(accommodationId);
            DeclinedReservations = await BookingLogic.GetAllDeclinedReservationsForAccommodationAsync(accommodationId);
            CancelledReservations = await BookingLogic.GetAllCancelledReservationsForAccommodationAsync(accommodationId);

            if (accommodation.UserCanCancelBooking)
            {
                CancelledByUserReservations = await BookingLogic.GetAllCancelledByUserReservationsForAccommodationAsync(accommodationId);
            }

            return Page();

        }
    }
}
