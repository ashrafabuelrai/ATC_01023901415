using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Web.Models.DTOs.EventImageDTO
{
    public class EventImageDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int EventId { get; set; }
    }
}
