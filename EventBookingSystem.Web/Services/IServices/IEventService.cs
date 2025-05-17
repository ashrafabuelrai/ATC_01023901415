

using EventBookingSystem.Web.Models.DTOs.EventDTO;

namespace EventBookingSystem.Web.Services.IServices
{
    public interface IEventService
    {
        Task<T> GetAllEventsAsync<T>(string token);
        Task<T> GetEventByIdAsync<T>(int id, string token);
        Task<T> CreateEventAsync<T>(MultipartFormDataContent formData, string token);
        Task<T> UpdateEventAsync<T>(int Id,MultipartFormDataContent formData, string token);
        Task<T> DeleteEventAsync<T>(int id, string token);
        Task<T> GetAllEventsByCategoryIdAsync<T>(int categoryId, string token);
    }
}
