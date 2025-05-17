using EventBookingSystem.Application.Common;
using EventBookingSystem.Application.Common.DTOs.BookingDTO;
using EventBookingSystem.Application.Common.DTOs.CategoryDTO;
using EventBookingSystem.Application.Contract;
using EventBookingSystem.Application.Services.Implementation;
using EventBookingSystem.Application.Services.Interface;
using EventBookingSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ITranslationService _translationService;
        private ApiResponse _apiResponse;
        public CategoryController(ICategoryService categoryService,ITranslationService translationService)
        {
            _categoryService = categoryService;
            _translationService = translationService;
            _apiResponse = new ApiResponse();
        }
        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetCategories()
        {
            var categories= await _categoryService.GetAllCategories();
            if(categories==null||categories.Count()==0)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string>() { "The Categories Not Found" };
                return NotFound(_apiResponse);
            }
            _apiResponse.Result = categories;
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if(category is null)
            {
                _apiResponse.ErrorMessage = new List<string> { "Category Not Found" };
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }
            _apiResponse.Result = category;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        // POST: api/Category
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> CreateCategory([FromBody] CategoryCreateDTO categoryData)
        {
            if (categoryData is null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }
            if(_categoryService.IsUniqueCategory(categoryData.Name).Result==false)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessage = new List<string>() { "Category already exists" };
                return BadRequest(_apiResponse);
            }
            categoryData.NameEN = await _translationService.TranslateAsync(categoryData.Name);
            await _categoryService.CreateCategory(categoryData);
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = categoryData;
            _apiResponse.StatusCode = HttpStatusCode.Created;
            return Ok(_apiResponse);
        }
        // PUT: api/Category/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> UpdateCategory([FromRoute]int id, [FromBody] CategoryUpdateDTO categoryData)
        {
            var category = await _categoryService.GetCategoryById(id);
            if(category is null || categoryData is null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }
            categoryData.NameEN = await _translationService.TranslateAsync(categoryData.Name);
            await _categoryService.UpdateCategory(id, categoryData);
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = categoryData;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category is null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessage = new List<string>() { "Category Not Found" };
                return BadRequest(_apiResponse);
            }
            await _categoryService.DeleteCategory(id);
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
    }
}
