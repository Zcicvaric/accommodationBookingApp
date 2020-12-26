using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly UserLogic userLogic;

        public LogoutModel(UserManager<ApplicationUser> userManager,
                          SignInManager<ApplicationUser> signInManager)
        {
            userLogic = new UserLogic(userManager, signInManager);
        }
        public async Task<IActionResult> OnGetAsync()
        {
            await userLogic.SignOutUserAsync();

            return RedirectToPage("/Index");
        }
    }
}
