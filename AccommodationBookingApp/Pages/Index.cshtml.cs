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
    public class IndexModel : PageModel
    {
        private AccommodationLogic AccommodationLogic = new AccommodationLogic();
        public List<Accommodation> Accommodations { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            Accommodations = await AccommodationLogic.GetAccommodations();

            return Page();
        }
    }
}