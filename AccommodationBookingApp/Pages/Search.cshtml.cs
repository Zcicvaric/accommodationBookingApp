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
        public async Task<IActionResult> OnGet()
        {
            //AccommodationLogic = new AccommodationLogic();
            //Accommodations = await AccommodationLogic.GetAccommodations();

            AccommodationTypeLogic = new AccommodationTypeLogic();
            AccommodationTypes = await AccommodationTypeLogic.GetAccommodationTypes();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            AccommodationTypeLogic = new AccommodationTypeLogic();
            AccommodationTypes = await AccommodationTypeLogic.GetAccommodationTypes();

            AccommodationLogic = new AccommodationLogic();
            Accommodations = await AccommodationLogic.GetFilteredAccommodations(AccommodationCity,
                             AccommodationTypeId, NumberOfGuests);
            return Page();
        }
    }
}
