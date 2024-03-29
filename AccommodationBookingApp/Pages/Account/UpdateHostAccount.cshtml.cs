using AccommodationBookingApp.BLL.UserLogic;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Pages
{
    [Authorize(Roles = "Host")]
    [BindProperties]
    public class UpdateHostAccountModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly UserLogic userLogic;

        [BindNever]
        public ApplicationUser ApplicationUser { get; set; }
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
        [RegularExpression(@"^([A-�a-�]{1,}\s?){1,}\s(\d{1,}[A-�a-�]{0,2})$", ErrorMessage = "Address needs to have the street name and street numbe, for example Kopilica 5")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Mobile Phone Number")]
        [RegularExpression(@"^(\d{9,10})$", ErrorMessage = "Mobile phone number must have 9 or 10 digits")]
        public string MobilePhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }

        public UpdateHostAccountModel(UserManager<ApplicationUser> userManager,
                                      SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

            userLogic = new UserLogic(userManager);
        }
        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser = await userManager.GetUserAsync(User);

            if (ApplicationUser == null)
            {
                return BadRequest();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ApplicationUser = await userManager.GetUserAsync(User);

            if (ApplicationUser == null)
            {
                return RedirectToPage("/UpdateHostAccount");
            }

            if (ModelState.IsValid)
            {
                var hostUpdateResult = await userLogic.UpdateHostAccountAsync(ApplicationUser, FirstName, LastName, Email, Address,
                                                                         MobilePhoneNumber, City, Country);

                if (hostUpdateResult.Succeeded)
                {
                    await signInManager.RefreshSignInAsync(ApplicationUser);
                    return RedirectToPage("/Account/Details");
                }

                foreach (var error in hostUpdateResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return Page();
            }

            return BadRequest();
        }
    }
}
