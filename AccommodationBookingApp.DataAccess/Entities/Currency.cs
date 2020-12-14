using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
