using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [Authorize (Roles = "Host")]
    public class EditAccommodationPhotosModel : PageModel
    {
        public Accommodation Accommodation { get; set; }
        public string HeaderPhotoPath { get; set; }
        public List<string> AccommodationPhotos { get; set; }
        [BindProperty]
        public string PhotoToDelete { get; set; }
        [BindProperty]
        public int AccommodationId { get; set; }
        [BindProperty]
        public IFormFile HeaderPhotoToUpload { get; set; }
        [BindProperty]
        public List<IFormFile> PhotosToUpload { get; set; }

        private readonly AccommodationLogic accommodationLogic = new AccommodationLogic();
        private readonly IWebHostEnvironment webHostEnviroment;

        public EditAccommodationPhotosModel(IWebHostEnvironment webHostEnviroment)
        {
            this.webHostEnviroment = webHostEnviroment;
        }

        public async Task<IActionResult> OnGetAsync(int accommodationId)
        {
            if (accommodationId == 0)
            {
                return BadRequest();
            }

            Accommodation = await accommodationLogic.GetAccommodationByIdAsync(accommodationId);

            if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
            {
                return Unauthorized();
            }

            var accommodationPhotosFolder = accommodationLogic.GetPhotosFolderPathForAccommodation(Accommodation);

            //removing the wwwroot prefix
            HeaderPhotoPath = accommodationPhotosFolder + "Header/" + Accommodation.HeaderPhotoFileName;

            HeaderPhotoPath = HeaderPhotoPath.Substring(7);


            try
            {
                AccommodationPhotos = Directory.GetFiles(accommodationPhotosFolder, "*", SearchOption.TopDirectoryOnly).ToList();
            }
            catch
            {

                AccommodationPhotos = new List<string>();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUploadNewHeaderPhoto()
        {
            if (ModelState.IsValid)
            {
                Accommodation = await accommodationLogic.GetAccommodationByIdAsync(AccommodationId);

                if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                var accommodationPhotosRootDirectory = Path.Combine(webHostEnviroment.WebRootPath, "accommodationPhotos");

                await accommodationLogic.UploadHeaderPhoto(Accommodation.Name, accommodationPhotosRootDirectory, Accommodation.Id, HeaderPhotoToUpload, true);

                return RedirectToPage("/EditAccommodationPhotos", new { accommodationId = AccommodationId });
            }

            return BadRequest();
        }

        public async Task<IActionResult> OnPostDeletePhotoAsync()
        {
            if (ModelState.IsValid)
            {
                Accommodation = await accommodationLogic.GetAccommodationByIdAsync(AccommodationId);

                if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                accommodationLogic.DeleteAccommodationPhoto(PhotoToDelete);

                return RedirectToPage("/EditAccommodationPhotos", new { accommodationId = AccommodationId });
            }

            return BadRequest();
        }

        public async Task<IActionResult> OnPostUploadPhotosAsync()
        {

            if (ModelState.IsValid)
            {
                Accommodation = await accommodationLogic.GetAccommodationByIdAsync(AccommodationId);

                if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                var accommodationPhotosRootDirectory = Path.Combine(webHostEnviroment.WebRootPath, "accommodationPhotos");

                await accommodationLogic.UploadAccommodationPhotos(Accommodation.Name, accommodationPhotosRootDirectory, AccommodationId, PhotosToUpload);

                return RedirectToPage("/EditAccommodationPhotos", new {accommodationId = AccommodationId});
            }

            return BadRequest();
        }
    }
}
