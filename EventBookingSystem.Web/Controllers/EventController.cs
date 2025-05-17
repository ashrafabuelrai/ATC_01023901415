
using AutoMapper;
using EventBookingSystem.Application.Common.Utility;
using EventBookingSystem.Domain.Entities;
using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Models.DTOs.BookingDTO;
using EventBookingSystem.Web.Models.DTOs.CategoryDTO;
using EventBookingSystem.Web.Models.DTOs.EventDTO;
using EventBookingSystem.Web.Models.ViewModels;
using EventBookingSystem.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using System.Configuration;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static EventBookingSystem.Application.Common.Utility.SD;
using static System.Net.Mime.MediaTypeNames;

namespace EventBookingSystem.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ICategoryService _categoryService;
        private readonly IEventImageService _eventImageService;
        public MultipartFormDataContent formData;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor httpAcc;

        public EventController(IEventService eventService,
            ICategoryService categoryService,
            IMapper mapper,
            IEventImageService eventImageService,
            IConfiguration configuration,
            IBookingService bookingService,
            IHttpContextAccessor httpAcc)
        {
            _eventService = eventService;
            _categoryService = categoryService;
            formData = new MultipartFormDataContent();
            _mapper = mapper;
            _eventImageService = eventImageService;
            _bookingService = bookingService;
            this.httpAcc = httpAcc;


        }
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            List<EventDTO> events = new List<EventDTO>();
            var respone = await _eventService.GetAllEventsAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess == true)
            {
                events = JsonConvert.DeserializeObject<List<EventDTO>>(Convert.ToString(respone.Result));
            }
            return View(events);
        }

        public async Task<IActionResult> Details(int Id)
        {
            var UserId = httpAcc.HttpContext.Session.GetString("UserId");
            EventVM eventVM = new EventVM();
            EventDTO Event = new EventDTO();
            var respone = await _eventService.GetEventByIdAsync<ApiResponse>(Id, HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess == true)
            {
                Event = JsonConvert.DeserializeObject<EventDTO>(Convert.ToString(respone.Result));
            }
            List<BookingDTO> bookings = new List<BookingDTO>();
            var response = await _bookingService.GetAllBookingsByUserIdAsync<ApiResponse>(UserId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                bookings = JsonConvert.DeserializeObject<List<BookingDTO>>(Convert.ToString(response.Result));
            }
            eventVM = new EventVM
            {
                Event = Event,
                IsBooked = bookings.Any(b => b.EventId == Id) ? true : false
            };
            return View(eventVM);
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create()
        {
            List<CategoryDTO> categories = new List<CategoryDTO>();
            var res = await _categoryService.GetAllCategoriesAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (res != null && res.IsSuccess)
            {
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(res.Result));
            }
            ViewBag.Categories = categories.Select(c => new { c.Id, c.Name }).ToList();

            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken] // اختياري لكن مستحب
        public async Task<IActionResult> Create(EventCreateDTO model)
        {
            var token = HttpContext.Session.GetString(SD.SessionToken);
            if (ModelState.IsValid)
            {
                var formData = new MultipartFormDataContent();

                formData.Add(new StringContent(model.Name ?? ""), "Name");
                formData.Add(new StringContent(model.Description ?? ""), "Description");
                formData.Add(new StringContent(model.Venue ?? ""), "Venue");
                formData.Add(new StringContent(model.Date.ToString("o")), "Date");
                formData.Add(new StringContent(model.Price.ToString()), "Price");
                formData.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");

                if (model.files != null && model.files.Any())
                {
                    foreach (var file in model.files)
                    {
                        var streamContent = new StreamContent(file.OpenReadStream());
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                        formData.Add(streamContent, "files", file.FileName);
                    }
                }


                var response = await _eventService.CreateEventAsync<ApiResponse>(formData, token);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "تم إنشاء الفعالية بنجاح";
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["error"] = "حدث خطأ أثناء الإنشاء";

            // إعادة تحميل التصنيفات عند الفشل
            List<CategoryDTO> categories = new List<CategoryDTO>();
            var res = await _categoryService.GetAllCategoriesAsync<ApiResponse>(token);
            if (res != null && res.IsSuccess)
            {
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(res.Result));
            }
            ViewBag.Categories = categories.Select(c => new { c.Id, c.Name }).ToList();

            return View(model);
        }
        [HttpGet()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int Id)
        {
            TempData["Id"] = Id;
            EventDTO Event = new EventDTO();
            var respone = await _eventService.GetEventByIdAsync<ApiResponse>(Id, HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess == true)
            {
                Event = JsonConvert.DeserializeObject<EventDTO>(Convert.ToString(respone.Result));
            }
            List<CategoryDTO> categories = new List<CategoryDTO>();
            var res = await _categoryService.GetAllCategoriesAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (res != null && res.IsSuccess)
            {
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(res.Result));
            }
            var model = _mapper.Map<EventUpdateDTO>(Event);
            EventUpdateVM eventUdateVM = new EventUpdateVM
            {
                categories = categories,
                Event = model,
                Id = Id,
                images = Event.Images

            };


            return View(eventUdateVM);
        }
        [HttpPost()]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(EventUpdateVM model)
        {
            var token = HttpContext.Session.GetString(SD.SessionToken);

            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new StringContent(model.Event.Name ?? ""), "Name");
            formData.Add(new StringContent(model.Event.Description ?? ""), "Description");
            formData.Add(new StringContent(model.Event.Venue ?? ""), "Venue");
            formData.Add(new StringContent(model.Event.Date.ToString("o")), "Date");
            formData.Add(new StringContent(model.Event.Price.ToString()), "Price");
            formData.Add(new StringContent(model.Event.CategoryId.ToString()), "CategoryId");


            if (model.Event.files != null && model.Event.files.Any())
            {
                foreach (var file in model.Event.files)
                {
                    var streamContent = new StreamContent(file.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    formData.Add(streamContent, "files", file.FileName);
                }
            }

            var response = await _eventService.UpdateEventAsync<ApiResponse>(model.Id, formData, token);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "تم تحديث الفعالية بنجاح";
                return RedirectToAction("Index", "Event");
            }

            TempData["error"] = "حدث خطأ أثناء التحديث";
            // إعادة تحميل التصنيفات عند الفشل
            
            

            return RedirectToAction("Update", "Event", new {Id=model.Id});
        }

        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {

            var result = await _eventService.DeleteEventAsync<ApiResponse>(Id, HttpContext.Session.GetString(SD.SessionToken));
            if (result != null && result.IsSuccess)
            {
                TempData["success"] = "تم حذف الحدث بنجاح";
                return RedirectToAction("Index", "Event");
            }
            TempData["success"] = "حدث خطأ أثناء الحذف";
            return RedirectToAction("Index", "Event");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteImage(int Id)
        {

            var response = await _eventImageService.DeleteEventImageByIdAsync<ApiResponse>(Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "تم حذف الصورة بنجاح";
                return RedirectToAction("Update", "Event", new { Id = TempData["Id"] });
            }

            TempData["error"] = "حدث خطأ أثناء حذف الصورة";
            return RedirectToAction("Update", "Event", new { Id = TempData["Id"] });


        }


    }
}
