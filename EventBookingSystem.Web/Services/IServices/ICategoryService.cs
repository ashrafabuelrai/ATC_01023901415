

using EventBookingSystem.Web.Models.DTOs.CategoryDTO;

namespace EventBookingSystem.Web.Services.IServices
{
    public interface ICategoryService
    {
        Task<T> GetAllCategoriesAsync<T>(string token);
        Task<T> GetCategoryByIdAsync<T>(int id, string token);
        Task<T> CreateCategoryAsync<T>(CategoryCreateDTO categoryDTO, string token);
        Task<T> UpdateCategoryAsync<T>(int id,CategoryUpdateDTO categoryDTO, string token);
        Task<T> DeleteCategoryAsync<T>(int id, string token);
    }
}
