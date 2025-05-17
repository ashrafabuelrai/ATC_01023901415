using AutoMapper;
using EventBookingSystem.Application.Common.DTOs.CategoryDTO;
using EventBookingSystem.Application.Common.DTOs.EventDTO;
using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Application.Services.Interface;
using EventBookingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateCategory(CategoryCreateDTO obj)
        {
            var category = _mapper.Map<Category>(obj);
            await _unitOfWork.Category.Add(category);
            await _unitOfWork.Save();
        }
        public async Task UpdateCategory(int Id, CategoryUpdateDTO obj)
        {
            var category = await _unitOfWork.Category.Get(x => x.Id == Id);
            _mapper.Map(obj,category);
            await _unitOfWork.Category.Update(category);
            await _unitOfWork.Save();
        }
        public async Task DeleteCategory(int id)
        {
            var category = await _unitOfWork.Category.Get(x => x.Id == id);
            await _unitOfWork.Category.Remove(category);
            await _unitOfWork.Save();

        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categories= await _unitOfWork.Category.GetAll();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }
        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category= await _unitOfWork.Category.Get(x => x.Id == id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<bool> IsUniqueCategory(string name)
        {
            var category = await _unitOfWork.Category.Get(c => c.Name == name);
            if (category == null)
                return true;
            return false;
        }
    }
   
}
