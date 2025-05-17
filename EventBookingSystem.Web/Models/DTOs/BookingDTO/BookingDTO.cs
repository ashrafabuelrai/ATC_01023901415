using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Web.Models.DTOs.BookingDTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime? BookingDate { get; set; }
        public int EventId { get; set; }
        
    }
}
