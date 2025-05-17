using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Domain.Entities
{
    public class EventImage
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
