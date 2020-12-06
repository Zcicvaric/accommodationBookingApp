using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    public class SearchModel : PageModel
    {

        private AccommodationLogic AccommodationLogic;
        private AccommodationTypeLogic AccommodationTypeLogic;
        public List<Accommodation> Accommodations { get; set; }
        public List<AccommodationType> AccommodationTypes { get; set; }
        [Required]
        [BindProperty]
        [Display (Name = "City")]
        public string AccommodationCity { get; set; }
        [Required]
        [BindProperty]
        [Display (Name = "Number of guests")]
        public int NumberOfGuests { get; set; }
        [Required]
        [BindProperty]
        [Display (Name = "Check-in date")]
        public string CheckInDateString { get; set; }
        [Required]
        [BindProperty]
        [Display (Name = "Check-out date")]
        public string CheckOutDateString { get; set; }
        public List<string> TimeOfTheDayList { get; set; }
        [BindProperty]
        public int AccommodationTypeId { get; set; }
        [BindProperty]
        public string LatestCheckInTime { get; set; }
        [BindProperty]
        public string EarliestCheckOutTime { get; set; }
        [BindProperty]
        public bool ShowOnlyAccommodationsWithInstantBooking { get; set; }
        [BindProperty]
        public bool ShowOnlyAccommodationsWhereUserCanCancelBooking { get; set; }
        public string ErrorMessage { get; set; }

        public SearchModel()
        {
            TimeOfTheDayList = new List<string>();

            for(int i = 0; i < 24; i++)
            {
                string timeOfTheDay = i.ToString() + ":00";
                TimeOfTheDayList.Add(timeOfTheDay);
            }
        }

        public async Task<IActionResult> OnGet()
        {
            //AccommodationLogic = new AccommodationLogic();
            //Accommodations = await AccommodationLogic.GetAccommodations();

            //you can't format this bellow to dd/mm/yyyy!!!!
            //maybe convert it everywhere to dd.mm.yyyy?
            CheckInDateString = DateTime.Today.ToShortDateString();
            CheckOutDateString = DateTime.Today.AddDays(1).ToShortDateString();
            AccommodationTypeLogic = new AccommodationTypeLogic();
            AccommodationTypes = await AccommodationTypeLogic.GetAccommodationTypes();
            return Page();
        }


        public async Task<IActionResult> OnPost()
        {

            AccommodationTypeLogic = new AccommodationTypeLogic();
            AccommodationTypes = await AccommodationTypeLogic.GetAccommodationTypes();

            DateTime checkInDate = DateTime.Parse(CheckInDateString);
            DateTime checkOutDate = DateTime.Parse(CheckOutDateString);

            AccommodationLogic = new AccommodationLogic();
            Accommodations = await AccommodationLogic.GetFilteredAccommodations(AccommodationCity, NumberOfGuests,
                                                                                checkInDate, checkOutDate, AccommodationTypeId,
                                                                                LatestCheckInTime, EarliestCheckOutTime,
                                                                                ShowOnlyAccommodationsWithInstantBooking,
                                                                                ShowOnlyAccommodationsWhereUserCanCancelBooking);

            if (Accommodations.Count == 0)
            {
                ErrorMessage = "Sorry, no accommodations found that match the given criteria";
            }
            return Page();
        }
    }
}
