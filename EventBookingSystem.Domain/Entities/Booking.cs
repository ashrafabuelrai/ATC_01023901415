using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingSystem.Domain.Entities
{
    public class Booking
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } 
        public ApplicationUser? User { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public int EventId { get; set; }
        public Event? Event { get; set; }
    }

}
