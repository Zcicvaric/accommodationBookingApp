using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AccommodationBookingApp.Pages
{
    public class RegisterModel : PageModel
    {
        
        [BindProperty]
        public ApplicationUser User { get; set; }
        [BindProperty]
        public string Password { get; set; }

        private UserLogic userLogic = new UserLogic();
        private readonly UserManager<ApplicationUser> userManager;

        public RegisterModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            if(ModelState.IsValid)
            {
                //userLogic.CreateNewUser(User, Password);
                Console.WriteLine("Its valid!");
                User.UserName = User.Email;
                var result = await userManager.CreateAsync(User, Password);
                if(result.Succeeded)
                {
                    return RedirectToPage("/index");
                }
                 
            }

            return RedirectToPage("/register");
            
        }
    }
}