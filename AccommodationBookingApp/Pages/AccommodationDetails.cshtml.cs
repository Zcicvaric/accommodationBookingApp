using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Hosting;
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
        public string HeaderPhotoPath { get; set; }
        public List<String> AccommodationPhotos { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        [BindProperty]
        public string CheckInDateString { get; set; }
        [BindProperty]
        public string CheckOutDateString { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public string [] DatesOccupiedArray { get; set; }

        private AccommodationLogic AccommodationLogic = new AccommodationLogic();
        private readonly UserManager<ApplicationUser> userManager;
        private BookingLogic BookingLogic = new BookingLogic();


        public AccommodationDetailsModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }


        public async Task<IActionResult> OnGet(int accommodationId, string checkInDate, string checkOutDate)
        {
            if (accommodationId == 0)
            {
                return BadRequest();
            }
            AccommodationId = accommodationId;
            Accommodation = await AccommodationLogic.GetAccommodationById(accommodationId);
            if (Accommodation == null)
            {
                return NotFound();
            }

            CurrentUser = await userManager.GetUserAsync(User);
            CheckInDateString = checkInDate;
            CheckOutDateString = checkOutDate;
            string dateformat = "dd.MM.yyyy.";
            try
            {
                CheckInDate = DateTime.ParseExact(checkInDate, dateformat, CultureInfo.InvariantCulture);
                CheckOutDate = DateTime.ParseExact(checkOutDate, dateformat, CultureInfo.InvariantCulture);

                if (CheckInDate < DateTime.Now.Date || CheckOutDate < DateTime.Now.AddDays(1).Date)
                {
                    throw new Exception("Invalid dates!");
                }
            }
            catch
            {
                CheckInDate = DateTime.Now;
                CheckOutDate = DateTime.Now.AddDays(1);

                CheckInDateString = CheckInDate.ToString(dateformat);
                CheckOutDateString = CheckOutDate.ToString(dateformat);
            }
            

            var ListOfDatesOccupied = await AccommodationLogic.GetDatesOccupiedForAccommodation(accommodationId);

            DatesOccupiedArray = ListOfDatesOccupied.ToArray();

            HeaderPhotoPath = "~/accommodationPhotos/" + Accommodation.Name + "/Header/" + Accommodation.HeaderPhotoFileName;

            string accommodationImagesFolder = "wwwroot/accommodationPhotos/" + Accommodation.Name + "/";

            AccommodationPhotos = Directory.GetFiles(accommodationImagesFolder, "*", SearchOption.TopDirectoryOnly).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                CurrentUser = await userManager.GetUserAsync(User);

                Accommodation = await AccommodationLogic.GetAccommodationById(AccommodationId);

                if (Accommodation.ApplicationUser.Id == CurrentUser.Id)
                {
                    return BadRequest();
                }

                var datesOccupied = await AccommodationLogic.GetDatesOccupiedForAccommodation(AccommodationId);

                if(datesOccupied.Contains(CheckInDateString) || datesOccupied.Contains(CheckOutDateString))
                {
                    return BadRequest();
                }

                CheckInDate = DateTime.Parse(CheckInDateString);
                CheckOutDate = DateTime.Parse(CheckOutDateString);

                var booking = await BookingLogic.CreateNewBooking(AccommodationId, CurrentUser.Id,
                                                    CheckInDate, CheckOutDate);

                if (booking.Id != 0)
                {
                    return RedirectToPage("/Bookings");
                }

            }

            return NotFound();
        }
    }
}
