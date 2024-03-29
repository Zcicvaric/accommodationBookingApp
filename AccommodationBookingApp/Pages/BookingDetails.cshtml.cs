using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages
{
    [Authorize]
    public class BookingDetailsModel : PageModel
    {
        private readonly BookingLogic bookingLogic;
        private readonly AccommodationLogic accommodationLogic;

        public Booking Booking { get; set; }
        public Accommodation Accommodation { get; set; }

        public BookingDetailsModel()
        {
            bookingLogic = new BookingLogic();
            accommodationLogic = new AccommodationLogic();
        }
        public async Task<IActionResult> OnGetAsync(int bookingId, int accommodationId)
        {
            if (bookingId == 0 || accommodationId == 0)
            {
                return BadRequest();
            }

            Booking = await bookingLogic.GetBookingByIdAsync(bookingId);
            Accommodation = await accommodationLogic.GetAccommodationByIdAsync(accommodationId);

            if (Booking == null || Accommodation == null)
            {
                return NotFound();
            }

            //check if a user is trying to access some other user's booking - in that case deny the request
            //but if a host is trying to see a booking for one of his accommodations, allow it
            if (User.IsInRole("User") && Booking.ApplicationUser.UserName != User.Identity.Name)
            {
                return Unauthorized();
            }
            if (User.IsInRole("Host") && Accommodation.ApplicationUser.UserName != User.Identity.Name)
            {
                return Unauthorized();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUserCancelBookingAsync(int bookingId)
        {

            if (ModelState.IsValid)
            {
                if (!User.IsInRole("User"))
                {
                    return Unauthorized();
                }
                Booking = await bookingLogic.GetBookingByIdAsync(bookingId);

                if (Booking == null)
                {
                    return NotFound();
                }

                if (Booking.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                var cancellationSuccessful = await bookingLogic.CancelBookingAsUserAsync(Booking.Id);

                if (cancellationSuccessful)
                {
                    return RedirectToPage("/Bookings");
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostHostApproveBookingAsync(int bookingId)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Host"))
                {
                    Booking = await bookingLogic.GetBookingByIdAsync(bookingId);

                    if (Booking == null)
                    {
                        return NotFound();
                    }

                    Accommodation = await accommodationLogic.GetAccommodationByIdAsync(Booking.Accommodation.Id);

                    if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                    {
                        return Unauthorized();
                    }

                    var approvalSuccessful = await bookingLogic.ApproveBookingAsync(bookingId);

                    if (approvalSuccessful)
                    {
                        return RedirectToPage("/Reservations");
                    }
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostHostDeclineBookingAsync(int bookingId)
        {
            if (ModelState.IsValid)
            {
                if (!User.IsInRole("Host"))
                {
                    return Unauthorized();
                }

                Booking = await bookingLogic.GetBookingByIdAsync(bookingId);

                if (Booking == null)
                {
                    return NotFound();
                }

                Accommodation = await accommodationLogic.GetAccommodationByIdAsync(Booking.Accommodation.Id);

                if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                var declineSuccessful = await bookingLogic.DeclineBookingAsync(bookingId);

                if (declineSuccessful)
                {
                    return RedirectToPage("/Reservations");
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostHostCancelBookingAsync(int bookingId)
        {
            if (ModelState.IsValid)
            {
                if (!User.IsInRole("Host"))
                {
                    return Unauthorized();
                }

                Booking = await bookingLogic.GetBookingByIdAsync(bookingId);

                if (Booking == null)
                {
                    return NotFound();
                }

                Accommodation = await accommodationLogic.GetAccommodationByIdAsync(Booking.Accommodation.Id);

                if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                var cancellationSuccessful = await bookingLogic.CancelBookingAsync(bookingId);

                if (cancellationSuccessful)
                {
                    return RedirectToPage("/Reservations");
                }
            }

            return NotFound();
        }
    }
}
