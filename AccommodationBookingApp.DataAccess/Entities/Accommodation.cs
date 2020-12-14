using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public class Accommodation
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
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
        public string CheckInTime { get; set; }
        [Required]
        public string CheckOutTime { get; set; }
        [Required]
        public bool UserCanCancelBooking { get; set; }
    }
}
