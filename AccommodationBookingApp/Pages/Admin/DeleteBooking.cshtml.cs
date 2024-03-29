using AccommodationBookingApp.BLL.AccommodationLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DeleteBookingModel : PageModel
    {
        [BindProperty]
        public int BookingId { get; set; }

        private readonly BookingLogic bookingLogic = new BookingLogic();

        public IActionResult OnGet(int bookingId)
        {
            if (bookingId == 0)
            {
                return BadRequest();
            }

            BookingId = bookingId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int bookingId)
        {
            if (ModelState.IsValid)
            {
                var result = await bookingLogic.DeleteBookingAsync(bookingId);

                if (result)
                {
                    return RedirectToPage("/Index");
                }
            }

            return BadRequest();
        }
    }
}
