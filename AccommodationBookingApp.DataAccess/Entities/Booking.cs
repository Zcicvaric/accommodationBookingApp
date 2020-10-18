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
        public int Id { get; set; }
        [ForeignKey("ApplicationUserId")]
        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("AccommodationId")]
        [Required]
        public int AccommodationId { get; set; }
        [Required]
        public DateTime DateOfArrival { get; set; }
        [Required]
        public int NumberOfDaysStaying { get; set; }
        [Required]//dodat in progress/approved/declined
        public bool ApprovalStatus { get; set; }
    }
}
