using AutoMapper;
using Azure;
using EventBookingSystem.Application.Common.Utility;
using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Models.DTOs.CategoryDTO;
using EventBookingSystem.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EventBookingSystem.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryService categoryService,IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<CategoryDTO> categories = new List<CategoryDTO>();
            var response = await _categoryService.GetAllCategoriesAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
            }

            return View(categories);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _categoryService.CreateCategoryAsync<ApiResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "تم إنشاء  الفئة بنجاح";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["error"] = "حدث خطأ أثناء الإنشاء";
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int Id)
        {
            CategoryDTO categoryDTO = new CategoryDTO();
            var response =await  _categoryService.GetCategoryByIdAsync<ApiResponse>(Id, HttpContext.Session.GetString(SD.SessionToken));
            if(response !=null && response.IsSuccess)
            {
                categoryDTO= JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result));
            }
            var category=_mapper.Map<CategoryUpdateDTO>(categoryDTO);
            ViewBag.id = Id;
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDTO model)
        {
            if(ModelState.IsValid)
            {
                var response = await _categoryService.UpdateCategoryAsync<ApiResponse>(id,model, HttpContext.Session.GetString(SD.SessionToken));
                if(response!=null && response.IsSuccess)
                {
                    TempData["success"] = "تم تحديث الفئة بنجاح";
                    return RedirectToAction("Index", "Category");
                }

            }
            TempData["error"] = "حدث خطأ أثناء التحديث";
            return View(model);
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = await _categoryService.DeleteCategoryAsync<ApiResponse>(Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "تم حذف الفئة بنجاح";
                return RedirectToAction("Index", "Category");
            }
            TempData["error"] = "حدث خطأ أثناء الحذف";
            return RedirectToAction("Index", "Category");
        }
    }
}
