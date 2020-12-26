using System.ComponentModel.DataAnnotations;

namespace AccommodationBookingApp.DataAccess.Entities
{
    public class Currency
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(3)]
        public string Name { get; set; }
    }
}
