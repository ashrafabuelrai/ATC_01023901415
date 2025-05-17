using AutoMapper;
using EventBookingSystem.Application.Common.DTOs.BookingDTO;
using EventBookingSystem.Application.Common.DTOs.CategoryDTO;
using EventBookingSystem.Application.Common.DTOs.EventDTO;
using EventBookingSystem.Application.Common.DTOs.EventImageDTO;
using EventBookingSystem.Application.Common.DTOs.UserDTO;
using EventBookingSystem.Domain.Entities;


namespace EventBookingSystem.API
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            //EventMapping
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<Event, EventCreateDTO>().ReverseMap();
            CreateMap<Event, EventUpdateDTO>().ReverseMap();
            //CategoryMapping
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();
            //BookingMapping
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<Event, BookingCreateDTO>().ReverseMap();
            //UserMapping
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
            //EventImage
            CreateMap<EventImage, EventImageDTO>().ReverseMap();
        }
    }
}
