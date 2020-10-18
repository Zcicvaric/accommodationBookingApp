using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public List<IFormFile> formFiles { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public List<string> TimesList { get; set; }

        private AccommodationLogic AccommodationLogic = new AccommodationLogic();
        private AccommodationTypeLogic AccommodationTypeLogic = new AccommodationTypeLogic();
        public List<AccommodationType> AccommodationTypes;

        public CreateAccommodationModel(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            TimesList = new List<string>();
        }
        public async Task<IActionResult> OnGet()
        {
            //var test  = await AccommodationTypeLogic.GetAccommodationTypes();
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
            if(ModelState.IsValid)
            {
                string headerImageFileName = null;
                if (formFiles != null && formFiles.Count > 0)
                {
                    string accommodationImagesFolder = Path.Combine(WebHostEnvironment.WebRootPath, "accommodationImages");
                    string directoryPath = Path.Combine(accommodationImagesFolder, Accommodation.Name);
                    System.IO.Directory.CreateDirectory(directoryPath);

                    foreach (IFormFile formFile in formFiles)
                    {
                        headerImageFileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
                        string filePath = Path.Combine(accommodationImagesFolder, Accommodation.Name, headerImageFileName);
                        formFile.CopyTo(new FileStream(filePath, FileMode.Create));
                        Accommodation.HeaderPhotoFileName = headerImageFileName;
                    }
                    
                }

                var result = await AccommodationLogic.CreateNewAccomodation(Accommodation);

                if(result)
                {
                    RedirectToPage("/CreateAccommodation");
                }    
            }
            return RedirectToPage("/index");
        }
    }
}