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
    public class CreateAccommodationModel : PageModel
    {
        [BindProperty]
        public Accommodation Accommodation { get; set; }

        private AccommodationLogic AccommodationLogic = new AccommodationLogic();
        private AccommodationTypeLogic AccommodationTypeLogic = new AccommodationTypeLogic();
        public List<AccommodationType> AccommodationTypes = new List<AccommodationType>();
        [BindProperty]
        public int testId { get; set; }


        public async Task<IActionResult> OnGet()
        {
           // var test  = await AccommodationTypeLogic.GetAccommodationTypes();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
                Accommodation.AccommodationType = AccommodationTypes[testId];
                var result = await AccommodationLogic.CreateNewAccomodation(Accommodation);
                if(result)
                {
                    RedirectToPage("/CreateAccommodation");
                }    
            }
            return RedirectToPage("/index");
        }
    }
}