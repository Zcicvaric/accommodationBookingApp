using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.CurrencyLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages.Admin
{
    [Authorize (Roles = "Admin")]
    public class CreateNewCurrencyModel : PageModel
    {
        [Required]
        [BindProperty]
        [Display (Name = "Currency Name")]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "Currency name must have exacly have exacly 3 upper case characters")]
        public string CurrencyName { get; set; }

        private CurrencyLogic currencyLogic = new CurrencyLogic();

        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await currencyLogic.AddCurrencyAsync(CurrencyName);

                if (result)
                {
                    return RedirectToPage("/Admin/CurrencyCreated");
                }
                ModelState.AddModelError("CurrencyName", "This currency name is already in use");

                return Page();
            }

            return BadRequest();
        }
    }
}
