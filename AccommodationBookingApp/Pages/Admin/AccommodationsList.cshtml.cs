using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AccommodationsListModel : PageModel
    {
        public List<Accommodation> Accommodations { get; set; }

        private readonly AccommodationLogic accommodationLogic = new AccommodationLogic();
        public async Task<IActionResult> OnGetAsync()
        {
            Accommodations = await accommodationLogic.GetAccommodationsAsync();

            return Page();
        }
    }
}
