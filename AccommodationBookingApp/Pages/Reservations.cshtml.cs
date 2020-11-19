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
        public ApplicationUser CurrentUser { get; set; }

        public ReservationsModel(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            if(!User.IsInRole("Host"))
            {
                return RedirectToPage("/Login");
            }
            CurrentUser = await UserManager.GetUserAsync(User);
            PendingReservations = await BookingLogic.GetAllPendingReservationsForHost(CurrentUser.Id);
            ApprovedReservations = await BookingLogic.GetAllApprovedReservationsForHost(CurrentUser.Id);
            PreviousReservations = await BookingLogic.GetAllPreviousReservationsForHost(CurrentUser.Id);
            DeclinedReservations = await BookingLogic.GetAllDeclinedReservationsForHost(CurrentUser.Id);
            CancelledReservations = await BookingLogic.GetAllCanceledReservationsForHost(CurrentUser.Id);
            

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
