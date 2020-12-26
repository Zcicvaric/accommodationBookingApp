using System.ComponentModel.DataAnnotations;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public class Accommodation
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string City { get; set; }
        [Required]
        [MaxLength(50)]
        public string Address { get; set; }
        [Required]
        public int NumberOfBeds { get; set; }
        [Required]
        public int PricePerNight { get; set; }
        [Required]
        public Currency Currency { get; set; }
        [Required]
        //require the booking to be confirmed by the host before its finalized
        public bool RequireApproval { get; set; }
        [Required]
        public AccommodationType AccommodationType { get; set; }
        [Required]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string HeaderPhotoFileName { get; set; }
        [Required]
        [MaxLength(5)]
        public string CheckInTime { get; set; }
        [Required]
        [MaxLength(5)]
        public string CheckOutTime { get; set; }
        [Required]
        public bool UserCanCancelBooking { get; set; }
    }
}
