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
    [Authorize (Roles = "Host")]
    public class ReservationsModel : PageModel
    {
        private UserManager<ApplicationUser> UserManager;
        private BookingLogic BookingLogic = new BookingLogic();
        [BindProperty]
        public List<Booking> PendingReservations { get; set; }
        [BindProperty]
        public List<Booking> ApprovedReservations { get; set; }
        [BindProperty]
        public List<Booking> PreviousReservations { get; set; }
        public List<Booking> DeclinedReservations { get; set; }
        public List<Booking> CancelledReservations { get; set; }
        public List<Booking> CancelledByUserReservations { get; set; }
        public ApplicationUser CurrentUser { get; set; }

        public ReservationsModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            CurrentUser = await UserManager.GetUserAsync(User);
            PendingReservations = await BookingLogic.GetAllPendingReservationsForHost(CurrentUser.Id);
            ApprovedReservations = await BookingLogic.GetAllApprovedReservationsForHost(CurrentUser.Id);
            PreviousReservations = await BookingLogic.GetAllPreviousReservationsForHost(CurrentUser.Id);
            DeclinedReservations = await BookingLogic.GetAllDeclinedReservationsForHost(CurrentUser.Id);
            CancelledReservations = await BookingLogic.GetAllCancelledReservationsForHost(CurrentUser.Id);
            CancelledByUserReservations = await BookingLogic.GetAllCancelledByUserReservationsForHost(CurrentUser.Id);
            

            return Page();
        }

        public async Task<IActionResult> OnGetForAccommodation(int accommodationId)
        {
            var accommodationLogic = new AccommodationLogic();
            var accommodation = await accommodationLogic.GetAccommodationById(accommodationId);

            CurrentUser = await UserManager.GetUserAsync(User);

            if (accommodation.ApplicationUser.Id != CurrentUser.Id)
            {
                return RedirectToPage("/Login");
            }

            PendingReservations = await BookingLogic.GetAllPendingReservationsForAccommodation(accommodationId);
            ApprovedReservations = await BookingLogic.GetAllApprovedReservationsForAccommodation(accommodationId);
            PreviousReservations = await BookingLogic.GetAllPreviousReservationsForAccommodation(accommodationId);
            DeclinedReservations = await BookingLogic.GetAllDeclinedReservationsForAccommodation(accommodationId);
            CancelledReservations = await BookingLogic.GetAllCancelledReservationsForAccommodation(accommodationId);
            if (accommodation.UserCanCancelBooking)
            {
                CancelledByUserReservations = await BookingLogic.GetAllCancelledByUserReservationsForAccommodation(accommodationId);
            }
            
            return Page();
            
        }
        public async Task<IActionResult> OnPostApproveBooking(int bookingId)
        {
            var result = await BookingLogic.ApproveBooking(bookingId);

            if(result)
            {
                //RedirectToPage is used to make it run the onGet method again to initialise the data once again
                return RedirectToPage("/Reservations");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostDeclineBooking(int bookingId)
        {
            var result = await BookingLogic.DeclineBooking(bookingId);

            if(result)
            {
                return RedirectToPage("/Reservations");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCancelBooking(int bookingId)
        {
            var result = await BookingLogic.CancelBooking(bookingId);

            if(result)
            {
                return RedirectToPage("/Reservations");
            }

            return Page();
        }
    }
}
