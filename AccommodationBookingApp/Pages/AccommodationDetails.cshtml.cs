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
        [BindProperty]
        public DateTime CheckInDate { get; set; }
        [BindProperty]
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
            CurrentUser = await userManager.GetUserAsync(User);
            var test = await AccommodationLogic.GetDatesOccupiedForAccommodation(AccommodationId);
            DatesOccupiedArray = test.ToArray();
            CheckInDate = DateTime.Parse(CheckInDateString);
            CheckOutDate = DateTime.Parse(CheckOutDateString);
            await BookingLogic.CreateNewBooking(AccommodationId, CurrentUser.Id,
                                                CheckInDate, CheckOutDate);
            return RedirectToPage("/Bookings");
        }
    }
}
