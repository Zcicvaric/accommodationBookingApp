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
    public class AccommodationDetailsModel : PageModel
    {
        [BindProperty]
        public int AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; }
        public string ImagePath { get; set; }
        public int MyProperty { get; set; }
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
            CurrentUser = await userManager.GetUserAsync(User);
            CheckInDateString = checkInDate;
            CheckOutDateString = checkOutDate;
            CheckInDate = DateTime.Now;
            CheckOutDate = DateTime.Now.AddDays(1);
            var test = await AccommodationLogic.GetDatesOccupiedForAccommodation(accommodationId);

            var ListOfDatesOccupied = new List<string>();

            DatesOccupiedArray = test.ToArray();

            ImagePath = "~/accommodationImages/" + Accommodation.Name + "/" + Accommodation.HeaderPhotoFileName;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            CurrentUser = await userManager.GetUserAsync(User);
            var test = await AccommodationLogic.GetDatesOccupiedForAccommodation(AccommodationId);
            DatesOccupiedArray = test.ToArray();
            //validacija u slucaju ako u get parameter posalje nevaljane datume
            if(DatesOccupiedArray.Contains(CheckInDateString) || DatesOccupiedArray.Contains(CheckOutDateString))
            {
                return Page();
            }
            CheckInDate = DateTime.Parse(CheckInDateString);
            CheckOutDate = DateTime.Parse(CheckOutDateString);
            await BookingLogic.CreateNewBooking(AccommodationId, CurrentUser.Id,
                                                CheckInDate, CheckOutDate);
            return RedirectToPage("/Bookings");
        }
    }
}
