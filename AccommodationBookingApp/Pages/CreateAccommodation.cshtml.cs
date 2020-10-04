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
    public class CreateAccommodationModel : PageModel
    {
        [BindProperty]
        public Accommodation Accommodation { get; set; }

        private AccommodationLogic AccommodationLogic = new AccommodationLogic();
        private AccommodationTypeLogic AccommodationTypeLogic = new AccommodationTypeLogic();
        public List<AccommodationType> AccommodationTypes;

        public CreateAccommodationModel()
        {

        }
        public async Task<IActionResult> OnGet()
        {
            //var test  = await AccommodationTypeLogic.GetAccommodationTypes();

            AccommodationTypes = await AccommodationTypeLogic.GetAccommodationTypes();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
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