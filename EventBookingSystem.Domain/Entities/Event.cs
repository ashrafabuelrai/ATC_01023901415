using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingSystem.Domain.Entities
{

    public class Event
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Venue { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Price { get; set; }
        //English 
        public string NameEN { get; set; }
        [Required]
        public string DescriptionEN { get; set; }
        [Required]
        public string VenueEN { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<EventImage>? EventImages { get; set; }
    }


}
