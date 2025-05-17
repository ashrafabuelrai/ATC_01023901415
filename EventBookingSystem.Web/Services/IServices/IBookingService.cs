using EventBookingSystem.Web.Models.DTOs.BookingDTO;

namespace EventBookingSystem.Web.Services.IServices
{
    public interface IBookingService
    {
        Task<T> GetAllBookingsAsync<T>(string token);
        Task<T> GetBookingByIdAsync<T>(int id, string token);
        Task<T> CreateBookingAsync<T>(BookingCreateDTO bookingDTO, string token);
        Task<T> GetAllBookingsByUserIdAsync<T>(string UserId, string token);

    }
}
