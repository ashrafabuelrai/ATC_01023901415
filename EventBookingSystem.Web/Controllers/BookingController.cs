
using Azure;
using EventBookingSystem.Application.Common.Utility;
using EventBookingSystem.Web.Models;
using EventBookingSystem.Web.Models.DTOs.BookingDTO;
using EventBookingSystem.Web.Models.DTOs.EventDTO;
using EventBookingSystem.Web.Models.ViewModels;
using EventBookingSystem.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EventBookingSystem.Web.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IEventService _eventService;
        public BookingController(IBookingService bookingService, IEventService eventService)
        {
            _bookingService = bookingService;
            _eventService = eventService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string UserId)

        {
            List<BookingVM> bookingsVM = new List<BookingVM>();
            List<BookingDTO> bookings = new List<BookingDTO>();
            var response = await _bookingService.GetAllBookingsByUserIdAsync<ApiResponse>(UserId, HttpContext.Session.GetString(SD.SessionToken));
            if(response!=null && response.IsSuccess)
            {
                bookings=JsonConvert.DeserializeObject<List<BookingDTO>>(Convert.ToString(response.Result));
            }
            foreach(var booking in bookings)
            {
                
                var res = await _eventService.GetEventByIdAsync<ApiResponse>(booking.EventId, HttpContext.Session.GetString(SD.SessionToken));
                EventDTO Event = new EventDTO();
                if(res!=null && res.IsSuccess)
                {
                    Event=JsonConvert.DeserializeObject<EventDTO>(Convert.ToString(res.Result));
                }
                BookingVM bookingVM = new BookingVM {
                    bookingDTO=booking,
                    EventName=Event.Name,
                    EventVenue=Event.Venue,
                    EventCategory=Event.Category,
                    EventId=booking.EventId,
                    EventPrice=Event.Price,
                    EventDate=Event.Date,
                    //English
                    EventCategoryEN=Event.CategoryEN,
                    EventNameEN=Event.NameEN,
                    EventVenueEN=Event.VenueEN

                };
                bookingsVM.Add(bookingVM);

            }
           
            return View(bookingsVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookingCreateDTO bookingCreateDTO)
        {
            
            if (ModelState.IsValid)
            {
                var reponse = await _bookingService.CreateBookingAsync<ApiResponse>(bookingCreateDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (reponse != null && reponse.IsSuccess)
                {
                    TempData["success"] = "تم الحجز بنجاح";
                    return View();
                }
            }
            TempData["error"] = "حدث خطأ أثناء الحجز";
            return RedirectToAction("Index","Home");
        }
    }
}
