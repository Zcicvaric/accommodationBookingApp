using AccommodationBookingApp.BLL.AccommodationLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class CreateNewAccommodationTypeModel : PageModel
    {
        [Required(ErrorMessage = "Please enter a name")]
        [BindProperty]
        [MaxLength(50)]
        [Display(Name = "Accommodation Type Name")]
        public string AccommodationTypeName { get; set; }

        private readonly AccommodationTypeLogic accommodationTypeLogic = new AccommodationTypeLogic();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await accommodationTypeLogic.AddAccommodationTypeAsync(AccommodationTypeName);

                if (result)
                {
                    return RedirectToPage("/Admin/AccommodationTypeCreated", "AccommodationTypeCreated");
                }
                ModelState.AddModelError("AccommodationTypeName", "This name is already in use");
                return Page();
            }
            return BadRequest();
        }
    }
}
