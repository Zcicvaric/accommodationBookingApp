using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public enum ApprovalStatus
    {
        Approved, Pending, Declined
    }
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public Accommodation Accommodation { get; set; }
        [Required]
        public DateTime DateOfArrival { get; set; }
        [Required]
        public int NumberOfDaysStaying { get; set; }
        [Required]
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
