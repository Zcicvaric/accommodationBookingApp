using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.BLL.CurrencyLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [Authorize (Roles = "Host")]
    [BindProperties]
    public class UpdateAccommodationModel : PageModel
    {
        [BindNever]
        public Accommodation Accommodation { get; set; }
        public int AccommodationId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the number of beds")]
        [Range(1, 1000)]
        [Display (Name = "Number of beds")]
        public int NumberOfBeds { get; set; }
        [Required (ErrorMessage = "Please enter the price per night")]
        [Range(1, int.MaxValue)]
        [Display (Name = "Price per night")]
        public int PricePerNight { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        public bool RequireApproval { get; set; }
        public bool UserCanCancelBooking { get; set; }
        [Required]
        [Display (Name = "Check-in time")]
        public string CheckInTime { get; set; }
        [Required]
        [Display (Name = "Check-out time")]
        public string CheckOutTime { get; set; }
        [BindNever]
        public List<string> TimesList { get; set; }

        private readonly AccommodationLogic accommodationLogic = new AccommodationLogic();
        private readonly CurrencyLogic currencyLogic = new CurrencyLogic();
        private readonly IWebHostEnvironment webHostEnviroment;
        public List<Currency> Currencies;

        public UpdateAccommodationModel(IWebHostEnvironment webHostEnviroment)
        {
            this.webHostEnviroment = webHostEnviroment;

            TimesList = new List<string>();

            for (int i = 0; i < 24; i++)
            {
                string timeOfTheDay = i.ToString() + ":00";
                TimesList.Add(timeOfTheDay);
            }
        }

        public async Task<IActionResult> OnGetAsync(int accommodationId)
        {
            Accommodation = await accommodationLogic.GetAccommodationByIdAsync(accommodationId);

            if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
            {
                return Unauthorized();
            }

            Currencies = await currencyLogic.GetCurrenciesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                int checkInTimeInt;
                int checkOutTimeInt;

                try
                {
                    checkInTimeInt = int.Parse(CheckInTime.Split(":")[0]);
                    checkOutTimeInt = int.Parse(CheckOutTime.Split(":")[0]);
                }
                catch
                {

                    return BadRequest();
                }

                if (checkInTimeInt <= checkOutTimeInt)
                {
                    ModelState.AddModelError("CheckInTime", "Check-in time must be later than check-out time!");

                    Accommodation = await accommodationLogic.GetAccommodationByIdAsync(AccommodationId);
                    Currencies = await currencyLogic.GetCurrenciesAsync();

                    return Page();
                }

                var accommodationImagesFolderPath = Path.Combine(webHostEnviroment.WebRootPath, "accommodationPhotos");

                Accommodation = await accommodationLogic.GetAccommodationByIdAsync(AccommodationId);

                if (Accommodation.ApplicationUser.UserName != User.Identity.Name)
                {
                    return Unauthorized();
                }

                var result = await accommodationLogic.UpdateAccommodationAsync(AccommodationId, Name, NumberOfBeds, PricePerNight, CurrencyId,
                                                                               RequireApproval, UserCanCancelBooking, CheckInTime, CheckOutTime, 
                                                                               Accommodation.Name, accommodationImagesFolderPath);

                if (!result)
                {
                    return BadRequest();
                }

                return RedirectToPage("/Accommodations");
            }

            return BadRequest();
        }
    }
}
