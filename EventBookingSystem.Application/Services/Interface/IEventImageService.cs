using EventBookingSystem.Application.Common.DTOs.EventImageDTO;
using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Interface
{
    public  interface IEventImageService
    {
        Task DeleteEventImageById(int Id);
        Task<EventImageDTO> GetEventImageById(int Id);
        
    }
}
