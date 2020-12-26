using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public override string Email { get; set; }
        [Required]
        public override string UserName { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }
    }
}
