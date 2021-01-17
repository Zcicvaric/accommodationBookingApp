using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CurrencyCreatedModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Admin/CreateNewCurrency");
        }

        public IActionResult OnGetCurrencyCreated()
        {
            return Page();
        }
    }
}
