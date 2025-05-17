using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Services.IServices;
using System.Configuration;
using static EventBookingSystem.Application.Common.Utility.SD;

namespace EventBookingSystem.Web.Services
{
    public class EventImageService:BaseService, IEventImageService
    {
        
        private string eventUrl;
        public EventImageService(IHttpClientFactory httpClient,IConfiguration configuration) : base(httpClient)
        {
            eventUrl = configuration.GetValue<string>("EventAPI");
        }
        public Task<T> DeleteEventImageByIdAsync<T>(int Id, string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.DELETE,
                Url = eventUrl+ "/api/Event/DeleteImage?Id=" +Id,
                Token=token
            });
        }
       
    
    }
}
