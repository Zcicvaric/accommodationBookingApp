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
        public async Task<IActionResult> OnGet(int bookingId, int accommodationId)
        {
            if (bookingId == 0 || accommodationId == 0)
            {
                return BadRequest();
            }
            
            Booking = await bookingLogic.GetBookingByIdAsync(bookingId);
            Accommodation = await accommodationLogic.GetAccommodationById(accommodationId);

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

        public async Task<IActionResult> OnPostUserCancelBooking(int bookingId)
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

                var cancellationSuccessful = await bookingLogic.CancelBookingAsUser(Booking.Id);

                if (cancellationSuccessful)
                {
                    return RedirectToPage("/Bookings");
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostHostApproveBooking(int bookingId)
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

                    Accommodation = await accommodationLogic.GetAccommodationById(Booking.Accommodation.Id);

                    if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                    {
                        return Unauthorized();
                    }

                    var approvalSuccessful = await bookingLogic.ApproveBooking(bookingId);

                    if(approvalSuccessful)
                    {
                        return RedirectToPage("/Reservations");
                    }
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostHostDeclineBooking(int bookingId)
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

                Accommodation = await accommodationLogic.GetAccommodationById(Booking.Accommodation.Id);

                if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                var declineSuccessful = await bookingLogic.DeclineBooking(bookingId);

                if(declineSuccessful)
                {
                    return RedirectToPage("/Reservations");
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostHostCancelBooking(int bookingId)
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

                Accommodation = await accommodationLogic.GetAccommodationById(Booking.Accommodation.Id);

                if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                var cancellationSuccessful = await bookingLogic.CancelBooking(bookingId);

                if (cancellationSuccessful)
                {
                    return RedirectToPage("/Reservations");
                }
            }

            return NotFound();
        }
    }
}
