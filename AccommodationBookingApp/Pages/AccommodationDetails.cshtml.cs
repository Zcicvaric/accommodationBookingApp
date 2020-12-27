using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages
{
    public class AccommodationDetailsModel : PageModel
    {
        [BindProperty]
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public string HeaderPhotoPath { get; set; }
        public List<string> AccommodationPhotos { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please select a valid check-in date")]
        public string CheckInDateString { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Please select a valid check-out date")]
        public string CheckOutDateString { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        public string[] DatesOccupiedArray { get; set; }

        private readonly AccommodationLogic AccommodationLogic = new AccommodationLogic();
        private readonly UserManager<ApplicationUser> userManager;
        private readonly BookingLogic BookingLogic = new BookingLogic();


        public AccommodationDetailsModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }


        public async Task<IActionResult> OnGetAsync(int accommodationId, string checkInDate, string checkOutDate)
        {
            if (accommodationId == 0)
            {
                return BadRequest();
            }

            if (User.Identity.IsAuthenticated)
            {
                CurrentUser = await userManager.FindByEmailAsync(User.Identity.Name);
            }

            AccommodationId = accommodationId;
            Accommodation = await AccommodationLogic.GetAccommodationByIdAsync(accommodationId);

            if (Accommodation == null)
            {
                return NotFound();
            }

            CheckInDateString = checkInDate;
            CheckOutDateString = checkOutDate;
            var dateformat = "dd.MM.yyyy.";

            var listOfDatesOccupied = await AccommodationLogic.GetDatesOccupiedForAccommodationAsync(accommodationId);

            try
            {
                CheckInDate = DateTime.ParseExact(checkInDate, dateformat, CultureInfo.InvariantCulture);
                CheckOutDate = DateTime.ParseExact(checkOutDate, dateformat, CultureInfo.InvariantCulture);

                if (CheckInDate < DateTime.Now.Date || CheckOutDate < DateTime.Now.AddDays(1).Date
                    || CheckInDate >= CheckOutDate)
                {
                    throw new Exception("Invalid dates!");
                }

                if (!await AccommodationLogic.CheckIfBookingDatesAreValid(CheckInDate, CheckOutDate, accommodationId))
                {
                    throw new Exception("Dates are occupied!");
                }
            }
            catch
            {
                CheckInDate = DateTime.Now.Date;
                CheckOutDate = DateTime.Now.Date;

                CheckInDateString = CheckInDate.ToString(dateformat);
                CheckOutDateString = CheckOutDate.ToString(dateformat);
            }


            DatesOccupiedArray = listOfDatesOccupied.ToArray();

            HeaderPhotoPath = "~/accommodationPhotos/" + Accommodation.Name + "_" + Accommodation.Id.ToString() + "/Header/" + Accommodation.HeaderPhotoFileName;

            var accommodationImagesFolder = "wwwroot/accommodationPhotos/" + Accommodation.Name + "_" + Accommodation.Id.ToString() + "/";

            try
            {
                AccommodationPhotos = Directory.GetFiles(accommodationImagesFolder, "*", SearchOption.TopDirectoryOnly).ToList();
            }
            catch (Exception)
            {
                AccommodationPhotos = new List<string>();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                CurrentUser = await userManager.GetUserAsync(User);

                Accommodation = await AccommodationLogic.GetAccommodationByIdAsync(AccommodationId);

                if (Accommodation.ApplicationUser.Id == CurrentUser.Id)
                {
                    return BadRequest();
                }

                try
                {
                    CheckInDate = DateTime.Parse(CheckInDateString);
                    CheckOutDate = DateTime.Parse(CheckOutDateString);

                    if (CheckInDate < DateTime.Now.Date || CheckOutDate < DateTime.Now.AddDays(1).Date
                        || CheckInDate >= CheckOutDate)
                    {
                        return BadRequest();
                    }
                    if (!await AccommodationLogic.CheckIfBookingDatesAreValid(CheckInDate, CheckOutDate, AccommodationId))
                    {
                        return BadRequest();
                    }

                }
                catch (Exception)
                {
                    return BadRequest();
                }

                var booking = await BookingLogic.CreateNewBookingAsync(AccommodationId, CurrentUser.Id,
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
