using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        [BindProperty]
        public ApplicationUser User { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }
        public void OnGet()
        {

        }
        public void OnPost()
        {

        }
    }
}