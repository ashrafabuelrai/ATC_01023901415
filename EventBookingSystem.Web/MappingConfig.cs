using AutoMapper;

using EventBookingSystem.Domain.Entities;
using EventBookingSystem.Web.Models.DTOs.BookingDTO;
using EventBookingSystem.Web.Models.DTOs.CategoryDTO;
using EventBookingSystem.Web.Models.DTOs.EventDTO;
using EventBookingSystem.Web.Models.DTOs.UserDTO;


namespace EventBookingSystem.Web
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            //EventMapping
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<Event, EventCreateDTO>().ReverseMap();
            CreateMap<Event, EventUpdateDTO>().ReverseMap();
            CreateMap<EventDTO, EventUpdateDTO>().ReverseMap();
            //CategoryMapping
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();
            CreateMap<CategoryDTO, CategoryUpdateDTO>().ReverseMap();
            //BookingMapping
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<Booking, BookingCreateDTO>().ReverseMap();
            
            //UserMapping
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
