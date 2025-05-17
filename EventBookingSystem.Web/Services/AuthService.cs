
using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Models.DTOs.UserDTO;
using EventBookingSystem.Web.Services.IServices;
using static EventBookingSystem.Application.Common.Utility.SD;

namespace EventBookingSystem.Web.Services
{
    public class AuthService : BaseService,IAuthService
    {
        
        private string eventUrl;
        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            
            eventUrl = configuration.GetValue<string>("EventAPI");
        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Data = loginRequestDTO,
                Url = eventUrl + "/api/Auth/login"
            });
        }

        public async Task<T> RegisterAsync<T>(RegisterationRequestDTO registerationRequestDTO)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Data = registerationRequestDTO,
                Url = eventUrl + "/api/Auth/register"
            });
        }
    }
}
