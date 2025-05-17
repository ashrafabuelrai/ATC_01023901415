using EventBookingSystem.Application.Common.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Application.Services.Interface
{
    public interface IUserService
    {
        Task<bool> IsUniqueUser(string UserId);
        Task<UserDTO> GetUser(string UserId);
        Task<LoginResponsDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
