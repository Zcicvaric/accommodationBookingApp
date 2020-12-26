using System;
using System.ComponentModel.DataAnnotations;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public enum ApprovalStatus
    {
        Approved, Pending, Declined, Cancelled, CancelledByUser
    }
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public Accommodation Accommodation { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        [Required]
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
