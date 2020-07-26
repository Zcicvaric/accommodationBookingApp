using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Accommodation Accommodation { get; set; }
        [Required]
        public DateTime DateOfArrival { get; set; }
        [Required]
        public int NumberOfDaysStaying { get; set; }
        [Required]
        public bool approvalStatus { get; set; }
    }
}
