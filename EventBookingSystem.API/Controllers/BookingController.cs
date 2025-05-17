using EventBookingSystem.Application.Common;
using EventBookingSystem.Application.Common.DTOs.BookingDTO;
using EventBookingSystem.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private ApiResponse _apiResponse;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
            _apiResponse = new ApiResponse();
        }
        [HttpGet("GetBookingsByUserId")]
        public async Task<ActionResult<ApiResponse>> GetBookingsByUserId([FromQuery]string UserId)
        {
            var bookings = await _bookingService.GetAllBookingsByUserId(UserId);
            if (bookings == null || bookings.Count() == 0)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string>() { "The Bookings Not Found" };
                return NotFound(_apiResponse);
            }
            _apiResponse.Result = bookings;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetBookings()
        {
            var bookings = await _bookingService.GetAllBookings();
            _apiResponse.Result = bookings;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetBooking(int id)
        {
            var booking = await _bookingService.GetBookingById(id);
            if(booking==null)
            {
                _apiResponse.ErrorMessage = new List<string> { "Booking Not Found" };
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }
            _apiResponse.Result = booking;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateBooking([FromBody] BookingCreateDTO bookingData)
        {

            if (bookingData is null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }
            // if exsit
            await _bookingService.CreateBooking(bookingData);
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = bookingData;
            _apiResponse.StatusCode = HttpStatusCode.Created;
            return Ok(_apiResponse);

        }
    }
}
