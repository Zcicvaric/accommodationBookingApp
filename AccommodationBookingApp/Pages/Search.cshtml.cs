using System;
using System.Collections.Generic;
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
        [BindProperty]
        public string AccommodationCity { get; set; }
        [BindProperty]
        public int AccommodationTypeId { get; set; }
        [BindProperty]
        public int NumberOfGuests { get; set; }
        [BindProperty]
        public String CheckInDateString { get; set; }
        [BindProperty]
        public String CheckOutDateString { get; set; }
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
            Accommodations = await AccommodationLogic.GetFilteredAccommodations(AccommodationCity,
                             AccommodationTypeId, NumberOfGuests, checkInDate, checkOutDate);
            return Page();
        }
    }
}
