using EventBookingSystem.Application.Common;
using EventBookingSystem.Application.Common.DTOs.UserDTO;
using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventBookingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private ApiResponse _apiResponse;
        public AuthController(IUserService userService)
        {

            _apiResponse = new ApiResponse();
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginRequestDTO loginDTO)
        {
            var loginRespone = await _userService.Login(loginDTO);
            if (loginRespone.User == null || string.IsNullOrEmpty(loginRespone.Token))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string> { "User or password is incorrect" };
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = loginRespone;
            return Ok(_apiResponse);
        }
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterationRequestDTO registerDTO)
        {
            bool ifUserNameUnique = await _userService.IsUniqueUser(registerDTO.UserName);
            if (!ifUserNameUnique)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string> { "Username already exists" };
                return BadRequest(_apiResponse);
            }
            var user = await _userService.Register(registerDTO);
            if (user == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string> { "Error while registering..." };
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}

