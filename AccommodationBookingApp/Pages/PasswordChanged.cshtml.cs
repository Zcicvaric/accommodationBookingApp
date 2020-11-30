using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [Authorize]
    public class PasswordChangedModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/ChangePassword");
        }
        public IActionResult OnGetPasswordChanged()
        {
            return Page();
        }
    }
}
