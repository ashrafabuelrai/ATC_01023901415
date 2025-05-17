
using EventBookingSystem.Application.Common.Utility;
using EventBookingSystem.Domain.Entities;
using EventBookingSystem.Web.Models.DTOs.CategoryDTO;
using EventBookingSystem.Web.Models.DTOs.EventDTO;

namespace EventBookingSystem.Web.Models.ViewModels
{
    public class EventUpdateVM
    {
        public int Id { get; set; }
        public EventUpdateDTO Event { get; set; }
        public List<CategoryDTO>? categories { get; set; }
        public List<Image> images { get; set; }
    }

}
