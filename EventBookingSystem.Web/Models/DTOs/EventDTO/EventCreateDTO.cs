using EventBookingSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Web.Models.DTOs.EventDTO
{
    public class EventCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Venue { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public List<IFormFile> files { get; set; }
        //English 
        public string? NameEN { get; set; }
        
        public string? DescriptionEN { get; set; }
        
        public string? VenueEN { get; set; }
        
    }
}
