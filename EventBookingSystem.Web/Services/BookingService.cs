
using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Models.DTOs.BookingDTO;
using EventBookingSystem.Web.Services.IServices;
using static EventBookingSystem.Application.Common.Utility.SD;

namespace EventBookingSystem.Web.Services
{
    public class BookingService:BaseService ,IBookingService
    {
       
        private string eventUrl;
        public BookingService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            
            eventUrl = configuration.GetValue<string>("EventAPI");
        }

        public async Task<T> CreateBookingAsync<T>(BookingCreateDTO bookingDTO, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Data = bookingDTO,
                Url = eventUrl + "/api/Booking",
                Token = token
            });
        }

        public async Task<T> GetAllBookingsAsync<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = eventUrl + "/api/Booking",
                Token = token
            });
        }

        public async Task<T> GetAllBookingsByUserIdAsync<T>(string UserId, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = eventUrl + "/api/Booking/GetBookingsByUserId?UserId=" + UserId,
                Token = token
            });
        }

        public async Task<T> GetBookingByIdAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = eventUrl + "/api/Booking/" + id,
                Token = token
            });
        }
    }
    
}
