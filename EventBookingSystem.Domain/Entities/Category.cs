using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventBookingSystem.Domain.Entities
{
    public class Category
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //English
        public string NameEN { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}
