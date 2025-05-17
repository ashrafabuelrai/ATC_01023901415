
using EventBookingSystem.Application.Common.DTOs.CategoryDTO;
using EventBookingSystem.Application.Common.Utility;
using EventBookingSystem.Domain.Entities;
using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Models.DTOs.BookingDTO;
using EventBookingSystem.Web.Models.DTOs.EventDTO;
using EventBookingSystem.Web.Models.ViewModels;
using EventBookingSystem.Web.Services.IServices;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace EventBookingSystem.Web.Controllers 
{ 
    public class HomeController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ICategoryService _categoryService;
        private readonly IBookingService _bookingService;
        private readonly IHttpContextAccessor httpAcc;
        public HomeController(IEventService eventService,
            ICategoryService categoryService,
            IBookingService bookingService,
            IHttpContextAccessor httpAcc)
        {
            _eventService = eventService;
            _categoryService = categoryService;
            _bookingService = bookingService;
            this.httpAcc = httpAcc;
        }
        public async Task<IActionResult> Index(int? categoryId)
        {
            var UserId = httpAcc.HttpContext.Session.GetString("UserId");
            List<EventVM> eventVMs = new List<EventVM>();
            var lang = CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "ar" : "en";
            List<EventDTO> list = new List<EventDTO>();
            var respone = await _eventService.GetAllEventsAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<EventDTO>>(Convert.ToString(respone.Result));
            }
            list = list.Where(e => e.Date >= DateTime.Now).ToList();
            List<BookingDTO> bookings = new List<BookingDTO>();
            var response = await _bookingService.GetAllBookingsByUserIdAsync<ApiResponse>(UserId, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                bookings = JsonConvert.DeserializeObject<List<BookingDTO>>(Convert.ToString(response.Result));
            }
            foreach (var item in list)
            {
                EventVM eventVM = new EventVM
                {
                    Event=item,
                    IsBooked=bookings.Any(b => b.EventId == item.Id)? true : false
                };
                eventVMs.Add(eventVM);
            }
            List<CategoryDTO> categories = new List<CategoryDTO>();
            var res=   await _categoryService.GetAllCategoriesAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (res != null && res.IsSuccess)
            {
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(res.Result));
            }
            var categoryList = categories.Select(c => new
            {
                Id = c.Id,
                Name = lang == "ar" ? c.Name : c.NameEN
            }).ToList();
            ViewBag.Categories = new SelectList(categoryList, "Id", "Name");
            ViewBag.SelectedCategory = categoryId;
            if (categoryId.HasValue || !string.IsNullOrEmpty (categoryId.ToString()))
            {
                // Filter events by categoryId if one is selected
                List<EventVM> events = eventVMs.Where(e => e.Event.CategoryId == categoryId).ToList();
                return View(events);  // Or return PartialView if it's an AJAX call
            }

           
            return View(eventVMs);  // No category sele
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        // language switcher
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (User.IsInRole("Admin"))
            {
                culture = "ar";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
    }
}
