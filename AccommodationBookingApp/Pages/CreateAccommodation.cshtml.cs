﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.AccommodationLogic;
using AccommodationBookingApp.BLL.CurrencyLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    [Authorize (Roles = "Host")]
    public class CreateAccommodationModel : PageModel
    {
        
        [Required]
        [BindProperty]
        public string Name { get; set; }
        [Required]
        [BindProperty]
        public string City { get; set; }
        [Required]
        [BindProperty]
        [RegularExpression (@"^([A-Ža-ž]{1,}\s?){1,}\s(\d{1,}[A-Ža-ž]{0,2})$", ErrorMessage = "Address needs to have the street name and street number, for example: Kopilica 5")]
        public string Address { get; set; }
        [Required (ErrorMessage = "Please enter the number of beds")]
        [BindProperty]
        [Display (Name = "Number of beds")]
        public int NumberOfBeds { get; set; }
        [Required (ErrorMessage = "Please enter the price per night")]
        [BindProperty]
        [Display (Name = "Price per night")]
        public int PricePerNight { get; set; }
        [Required]
        [BindProperty]
        public int CurrencyId { get; set; }
        [BindProperty]
        public bool RequireApproval { get; set; }
        [BindProperty]
        public bool UserCanCancelBooking { get; set; }
        [Required (ErrorMessage = "Accommodation type is required")]
        [BindProperty]
        [Display (Name = "Accommodation type")]
        public int AccommodationTypeId { get; set; }
        [Required]
        [BindProperty]
        [Display (Name = "Check-in time")]
        public string CheckInTime { get; set; }
        [Required]
        [BindProperty]
        [Display (Name = "Check-out time")]
        public string CheckOutTime { get; set; }
        public object WebRootPath { get; private set; }
        [Required (ErrorMessage = "Please select one or more photos")]
        [BindProperty]
        [Display (Name = "Accommodation photos")]
        public List<IFormFile> AccommodationPhotos { get; set; }
        [Required (ErrorMessage = "Please select a header photo")]
        [BindProperty]
        [Display (Name = "Header photo")]
        public IFormFile AccommodationHeaderPhoto { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public List<string> TimesList { get; set; }

        private AccommodationLogic AccommodationLogic = new AccommodationLogic();
        private AccommodationTypeLogic AccommodationTypeLogic = new AccommodationTypeLogic();
        private CurrencyLogic CurrencyLogic = new CurrencyLogic();

        public List<AccommodationType> AccommodationTypes;
        public List<Currency> Currencies;


        public CreateAccommodationModel(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            TimesList = new List<string>();

            for (int i = 0; i < 24; i++)
            {
                string timeOfTheDay = i.ToString() + ":00";
                TimesList.Add(timeOfTheDay);
            }
        }
        public async Task<IActionResult> OnGet()
        {
            AccommodationTypes = await AccommodationTypeLogic.GetAccommodationTypes();
            Currencies = await CurrencyLogic.GetCurrenciesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            int checkInTimeInt;
            int checkOutTimeInt;

            if (ModelState.IsValid)
            {

                try
                {
                    checkInTimeInt = Int32.Parse(CheckInTime.Split(":")[0]);
                    checkOutTimeInt = Int32.Parse(CheckOutTime.Split(":")[0]);
                }
                catch
                {
                    return BadRequest();
                }
                if (checkInTimeInt <= checkOutTimeInt)
                {
                    ModelState.AddModelError("CheckInTime", "Check-in time must be later than check-out time!");
                    //this is required as the page requires accommodationTypes list populated in order to display the
                    //accommodation type select list and currency select list
                    AccommodationTypes = await AccommodationTypeLogic.GetAccommodationTypes();
                    Currencies = await CurrencyLogic.GetCurrenciesAsync();
                    return Page();
                }

                string accommodationImagesFolder = Path.Combine(WebHostEnvironment.WebRootPath, "accommodationPhotos");

                var result = await AccommodationLogic.CreateNewAccomodation(Name, City, Address, NumberOfBeds, PricePerNight, CurrencyId, RequireApproval,
                                                                            AccommodationTypeId, CheckInTime, CheckOutTime, User.Identity.Name, UserCanCancelBooking,
                                                                            accommodationImagesFolder, AccommodationHeaderPhoto, AccommodationPhotos);

                if (!result)
                {
                    return BadRequest();
                }

                return RedirectToPage("/Accommodation");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}