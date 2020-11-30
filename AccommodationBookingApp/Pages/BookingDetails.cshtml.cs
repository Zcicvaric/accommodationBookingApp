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
        private BookingLogic bookingLogic;
        private AccommodationLogic accommodationLogic;

        public Booking Booking { get; set; }
        public Accommodation Accommodation { get; set; }

        public BookingDetailsModel()
        {
            bookingLogic = new BookingLogic();
            accommodationLogic = new AccommodationLogic();
        }
        public async Task<IActionResult> OnGet(int bookingId, int accommodationId)
        {

            Booking = await bookingLogic.GetBookingByIdAsync(bookingId);
            Accommodation = await accommodationLogic.GetAccommodationById(accommodationId);

            //check if a user is trying to accsess some other user's booking - in that case deny the request
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
                Booking = await bookingLogic.GetBookingByIdAsync(bookingId);

                if (Booking == null)
                {
                    return NotFound();
                }

                if (Booking.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                var result = await bookingLogic.CancelBookingAsUser(Booking.Id);

                if (result)
                {
                    return RedirectToPage("/Bookings");
                }
            }

            return Page();
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

                    var approvalSuccesful = await bookingLogic.ApproveBooking(bookingId);

                    if(approvalSuccesful)
                    {
                        return RedirectToPage("/Bookings");
                    }
                }
            }

            return Page();
        }
    }
}
