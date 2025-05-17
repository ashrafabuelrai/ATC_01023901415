using EventBookingSystem.Web.Models;
namespace EventBookingSystem.Web.Services.IServices
{
    public interface IBaseService
    {
        ApiResponse responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
