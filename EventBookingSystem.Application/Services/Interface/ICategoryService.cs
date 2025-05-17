using EventBookingSystem.Application.Common.DTOs.CategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Interface
{
    public interface ICategoryService
    {
        Task<bool> IsUniqueCategory(string name);
        Task CreateCategory(CategoryCreateDTO categoryCreateDTO);
        Task UpdateCategory(int Id,CategoryUpdateDTO categoryUpdateDTO);
        Task DeleteCategory(int id);
        Task<CategoryDTO> GetCategoryById(int id);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
    }

    public class Enumerable<T>
    {
    }
}
