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
        //require the booking to be confirmed by the host before its finalized
        public bool RequireApproval { get; set; }
        [ForeignKey("AccommodationTypeId")]
        public int AccommodationTypeId { get; set; }
        public virtual AccommodationType AccommodationType { get; set; }
        [ForeignKey("ApplicationUserId")]
        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string HeaderPhotoFileName { get; set; }
        [Required]
        public string CheckInTime { get; set; }
        [Required]
        public string CheckOutTime { get; set; }
        //to add: facilities (Wi-Fi, TV...)
    }
}
