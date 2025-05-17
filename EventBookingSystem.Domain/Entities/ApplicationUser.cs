using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Domain.Entities
{

    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IEnumerable<Booking>? Bookings { get; set; }
    }

}
