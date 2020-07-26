using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
