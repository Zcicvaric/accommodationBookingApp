using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages.Admin
{
    [Authorize (Roles = "Admin")]
    public class AccommodationsListModel : PageModel
    {
        public List<Accommodation> Accommodations { get; set; }

        AccommodationLogic accommodationLogic = new AccommodationLogic();
        public async Task<IActionResult> OnGetAsync()
        {
            Accommodations = await accommodationLogic.GetAccommodations();

            return Page();
        }
    }
}
