using EventBookingSystem.Application.Common.DTOs.EventDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Interface
{
    public interface IEventService
    {
        Task CreateEvent(EventCreateDTO eventDto);
        Task<EventDTO> GetEventById(int Id);
        Task<IEnumerable<EventDTO>> GetAllEvents();
        Task UpdateEvent(int Id,EventUpdateDTO eventDto);
        Task DeleteEvent(int Id);
        Task<IEnumerable<EventDTO>> GetEventsByCategoryId(int categoryId);
        Task<IEnumerable<EventDTO>> GetEventsByUserId(string UserId);
    }
}
