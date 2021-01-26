using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    public class IndexModel : PageModel
    {

        public IActionResult OnGet()
        {
            return RedirectToPage("/Search");
        }
    }
}