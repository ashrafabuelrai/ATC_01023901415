using EventBookingSystem.Application.Common.Utility;
using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Common.DTOs.EventDTO
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Venue { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        [AllowNull]
        public List<Image>? Images { get; set; } = new List<Image>();
        //English 
        public string? NameEN { get; set; }
       
        public string? DescriptionEN { get; set; }
        
        public string? VenueEN { get; set; }
        
        public string? CategoryEN { get; set; }
    }
}
