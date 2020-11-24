using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    public class CreateAccommodationModel : PageModel
    {
        [BindProperty]
        public Accommodation Accommodation { get; set; }
        public object WebRootPath { get; private set; }
        public List<IFormFile> AccommodationPhotos { get; set; }
        public IFormFile AccommodationHeaderPhoto { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public List<string> TimesList { get; set; }
        [BindProperty]
        public int AccommodationTypeId { get; set; }

        private AccommodationLogic AccommodationLogic = new AccommodationLogic();
        private AccommodationTypeLogic AccommodationTypeLogic = new AccommodationTypeLogic();
        public List<AccommodationType> AccommodationTypes;
        private readonly UserManager<ApplicationUser> userManager;

        public CreateAccommodationModel(IWebHostEnvironment webHostEnvironment,
                                        UserManager<ApplicationUser> userManager)
        {
            WebHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            TimesList = new List<string>();
        }
        public async Task<IActionResult> OnGet()
        {
            if (!User.IsInRole("Host"))
            {
                return RedirectToPage("/Login");
            }
            for(int i = 0; i <= 24; i++)
            {
                string timeOfTheDay = i.ToString() + ":00";
                TimesList.Add(timeOfTheDay);
            }

            AccommodationTypes = await AccommodationTypeLogic.GetAccommodationTypes();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            ApplicationUser applicationUser = await userManager.GetUserAsync(User);

            string accommodationImagesFolder = Path.Combine(WebHostEnvironment.WebRootPath, "accommodationPhotos");

            var result = await AccommodationLogic.CreateNewAccomodation(Accommodation,
                                                                       AccommodationTypeId,
                                                                       applicationUser.Id,
                                                                       accommodationImagesFolder,
                                                                       AccommodationHeaderPhoto,
                                                                       AccommodationPhotos);
            if (!result)
            {
                return RedirectToPage("/CreateAccommodation");
            }

            return RedirectToPage("/index");
        }
    }
}