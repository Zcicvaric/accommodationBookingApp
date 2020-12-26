using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AccountsModel : PageModel
    {
        private readonly UserLogic UserLogic;
        public List<ApplicationUser> UserAccounts { get; set; }
        public List<ApplicationUser> HostAccounts { get; set; }

        public AccountsModel(UserManager<ApplicationUser> userManager)
        {
            UserLogic = new UserLogic(userManager);
        }
        public async Task<IActionResult> OnGetAsync()
        {
            UserAccounts = await UserLogic.GetAllUserAccountsAsync();
            HostAccounts = await UserLogic.GetAllHostAccountsAsync();

            return Page();
        }
    }
}
