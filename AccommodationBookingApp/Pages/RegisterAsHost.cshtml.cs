using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages
{
    [BindProperties]
    public class RegisterAsHostModel : PageModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^([A-Ža-ž]{1,}\s?){1,}\s(\d{1,}[A-Ža-ž]{0,2})$", ErrorMessage = "Address needs to have the street name and street numbe, for example: Kopilica 5")]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [BindNever]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Mobile Phone Number")]
        [RegularExpression(@"^(\d{9,10})$", ErrorMessage = "Mobile phone number must have 9 or 10 digits")]
        public string MobilePhoneNumber { get; set; }

        private readonly UserLogic userLogic;

        public RegisterAsHostModel(UserManager<ApplicationUser> userManager,
                                   SignInManager<ApplicationUser> signInManager,
                                   RoleManager<IdentityRole> roleManager)
        {
            userLogic = new UserLogic(userManager, signInManager, roleManager);
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await userLogic.CreateNewHostAsync(FirstName, LastName, Email, Address, City, Country, MobilePhoneNumber,
                                                           Password);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Index");
                }

                foreach (var registrationError in result.Errors)
                {
                    ModelState.AddModelError("", registrationError.Description);

                    return Page();
                }
            }

            return BadRequest();
        }
    }
}
