using EventBookingSystem.Application.Common;
using EventBookingSystem.Application.Common.DTOs.CategoryDTO;
using EventBookingSystem.Application.Common.DTOs.EventDTO;
using EventBookingSystem.Application.Contract;
using EventBookingSystem.Application.Services.Implementation;
using EventBookingSystem.Application.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IEventImageService _eventImageService;
        private readonly ITranslationService _translationService;
        private readonly ICategoryService _categoryService;
        private ApiResponse _apiResponse;
        public EventController(IEventService eventService,
            IEventImageService eventImageService,
            ITranslationService translationService,
            ICategoryService categoryService)
        {

            _apiResponse = new ApiResponse();
            _eventService = eventService;
            _eventImageService = eventImageService;
            _translationService = translationService;
            _categoryService = categoryService;
        }
        // GET: api/Event
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetEvents()
        {
            var events = await _eventService.GetAllEvents();
            if (events == null || events.Count() == 0)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string>() { "The Events Not Found" };
                return NotFound(_apiResponse);
            }
            _apiResponse.Result = events;
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetEvent(int id)
        {
            var Event = await _eventService.GetEventById(id);
            if (Event is null)
            {
                _apiResponse.ErrorMessage = new List<string> { "Event Not Found" };
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }
            _apiResponse.Result = Event;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        [HttpGet("GetEventsByCategoryId{categoryId}")]
        public async Task<ActionResult<ApiResponse>> GetEventsByCategoryId(int categoryId)
        {
            var Event = await _eventService.GetEventsByCategoryId(categoryId);
            if (Event is null)
            {
                _apiResponse.ErrorMessage = new List<string> { "Event Not Found" };
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }
            _apiResponse.Result = Event;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
        // POST: api/Event
        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<ApiResponse>> CreateEvent([FromForm] EventCreateDTO eventData)
        {
            if (eventData is null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }
            // if exsit
            //eventData.files = files;

            eventData.VenueEN = await _translationService.TranslateAsync(eventData.Venue);
            eventData.DescriptionEN = await _translationService.TranslateAsync(eventData.Description);
            eventData.NameEN = await _translationService.TranslateAsync(eventData.Name);
            await _eventService.CreateEvent(eventData);
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = eventData;
            _apiResponse.StatusCode = HttpStatusCode.Created;
            return Ok(_apiResponse);
        }
        // PUT: api/Event/5
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> UpdateEvent([FromRoute]int id, [FromForm] EventUpdateDTO eventData)
        {
            var Event = await _eventService.GetEventById(id);
            if (Event is null || eventData is null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }
            var category = await _categoryService.GetCategoryById(eventData.CategoryId);
            eventData.VenueEN = await _translationService.TranslateAsync(eventData.Venue);
            eventData.DescriptionEN = await _translationService.TranslateAsync(eventData.Description);
            eventData.NameEN = await _translationService.TranslateAsync(eventData.Name);
            eventData.CategoryEN= category.NameEN;
            
            await _eventService.UpdateEvent(id, eventData);
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = eventData;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
        // DELETE: api/Event/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> DeleteEvent([FromRoute] int id)
        {
            var Event = await _eventService.GetEventById(id);
            if (Event is null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessage = new List<string>() { "Event Not Found" };
                return BadRequest(_apiResponse);
            }
            await _eventService.DeleteEvent(id);
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpDelete("DeleteImage")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse>> DeleteImage([FromQuery]int Id)
        {
            var image = await _eventImageService.GetEventImageById(Id);
            if (image is null)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessage = new List<string>() { "Image Not Found" };
                return BadRequest(_apiResponse);
            }
            await _eventImageService.DeleteEventImageById(Id);
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }
    }
}
