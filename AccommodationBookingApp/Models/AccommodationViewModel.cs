using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationBookingApp.Models
{
    public class AccommodationViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int NumberOfBeds { get; set; }
        public int PricePerNight { get; set; }
    }
}
