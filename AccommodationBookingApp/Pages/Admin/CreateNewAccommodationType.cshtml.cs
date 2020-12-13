using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages.Admin
{
    [Authorize (Roles = "Admin")]
    public class CreateNewAccommodationTypeModel : PageModel
    {
        [Required]
        [BindProperty]
        [Display (Name = "Accommodation Type Name")]
        public string AccommodationTypeName { get; set; }

        private AccommodationTypeLogic accommodationTypeLogic = new AccommodationTypeLogic();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var result = await accommodationTypeLogic.AddAccommodationType(AccommodationTypeName);

                if (result)
                {
                    return RedirectToPage("/Admin/AccommodationTypeCreated");
                }
                ModelState.AddModelError("AccommodationTypeName", "This name is already in use");
                return Page();
            }
            return BadRequest();
        }
    }
}
