namespace EventBookingSystem.Web.Services.IServices
{
    public interface IEventImageService
    {
        Task<T> DeleteEventImageByIdAsync<T>(int Id,string token);
        
    }
}
