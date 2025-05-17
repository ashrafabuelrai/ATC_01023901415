using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Models.DTOs.CategoryDTO;
using EventBookingSystem.Web.Services.IServices;
using static EventBookingSystem.Application.Common.Utility.SD;

namespace EventBookingSystem.Web.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
       
        private string eventUrl;
        public CategoryService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
           
            eventUrl = configuration.GetValue<string>("EventAPI");
        }
        public async Task<T> GetAllCategoriesAsync<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = eventUrl + "/api/Category",
                Token = token
            });
        }
        public async Task<T> GetCategoryByIdAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = eventUrl + "/api/Category/" + id,
                Token = token
            });
        }
        public async Task<T> CreateCategoryAsync<T>(CategoryCreateDTO categoryDTO, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Data = categoryDTO,
                Url = eventUrl + "/api/Category",
                Token = token
            });
        }
        public async Task<T> UpdateCategoryAsync<T>(int id,CategoryUpdateDTO categoryDTO, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.PUT,
                Data = categoryDTO,
                Url = eventUrl + "/api/Category/"+id,
                Token = token
            });
        }
        public async Task<T> DeleteCategoryAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.DELETE,
                Url = eventUrl + "/api/Category/" + id,
                Token = token
            });
        }

       
    }
    
}
