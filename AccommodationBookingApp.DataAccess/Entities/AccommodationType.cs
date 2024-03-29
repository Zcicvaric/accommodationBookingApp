﻿using System.ComponentModel.DataAnnotations;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public class AccommodationType
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
