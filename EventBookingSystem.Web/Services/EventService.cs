using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Models.DTOs.EventDTO;
using EventBookingSystem.Web.Services.IServices;
using static EventBookingSystem.Application.Common.Utility.SD;

namespace EventBookingSystem.Web.Services
{
    public class EventService:BaseService,IEventService
    {
       
        private string eventUrl;
        public EventService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            
            eventUrl = configuration.GetValue<string>("EventAPI");
        }
        public async Task<T> CreateEventAsync<T>(MultipartFormDataContent formData, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Data = formData,
                Url = eventUrl + "/api/Event",
                Token = token
            });
        }
        public async Task<T> GetAllEventsAsync<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = eventUrl + "/api/Event",
                Token = token
            });
        }
        public async Task<T> GetEventByIdAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = eventUrl + "/api/Event/" + id,
                Token = token
            });
        }
        public async Task<T> UpdateEventAsync<T>(int Id,MultipartFormDataContent formData, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.PUT,
                Data = formData,
                Url = eventUrl + "/api/Event/"+Id,
                Token = token
            });
        }
        public async Task<T> DeleteEventAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.DELETE,
                Url = eventUrl + "/api/Event/" + id,
                Token = token
            });
        }
        public async Task<T> GetAllEventsByCategoryIdAsync<T>(int categoryId, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = eventUrl + "/api/Event/GetEventsByCategory?categoryId=" + categoryId,
                Token = token
            });
        }
    }
}
