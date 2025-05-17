
using EventBookingSystem.Web.Models.DTOs.EventDTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventBookingSystem.Web.Models.ViewModels
{
    public class EventVM
    {
       public EventDTO Event { get; set; }
       public bool IsBooked { get; set; }
    }
}
