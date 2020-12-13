using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting.Internal;

namespace AccommodationBookingApp.Pages
{
    [Authorize (Roles = "Host, Admin")]
    public class AccommodationModel : PageModel
    {
        private AccommodationLogic AccommodationLogic;
        private UserManager<ApplicationUser> UserManager;
        public List<Accommodation> Accommodations { get; set; }
        public string HeaderImageFolderPath { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public string Username { get; set; }

        public AccommodationModel(UserManager<ApplicationUser> userManager,
                                  IWebHostEnvironment webHostEnvironment)
        {
            AccommodationLogic = new AccommodationLogic();
            UserManager = userManager;
            WebHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGet()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Login");
            }
            Accommodations = await AccommodationLogic.GetAccommodationsWithUserId(user.Id);

            return Page();
        }

        public async Task<IActionResult> OnGetAccommodationsForHost(string username)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            var user = await UserManager.FindByNameAsync(username);

            Username = user.FirstName + " " + user.LastName;

            Accommodations = await AccommodationLogic.GetAccommodationsWithUserId(user.Id);

            return Page();
        }
    }
} 
