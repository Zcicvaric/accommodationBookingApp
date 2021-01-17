using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages.Admin
{
    [Authorize (Roles = "Admin")]
    public class AccommodationTypeCreatedModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Admin/CreateNewAccommodationType");
        }

        public IActionResult OnGetAccommodationTypeCreated()
        {
            return Page();
        }
    }
}
