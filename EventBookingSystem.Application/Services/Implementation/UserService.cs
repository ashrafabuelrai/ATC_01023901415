using AutoMapper;
using EventBookingSystem.Application.Common.DTOs.UserDTO;
using EventBookingSystem.Application.Common.Interfaces;
using EventBookingSystem.Application.Services.Interface;
using EventBookingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace EventBookingSystem.Application.Services.Implementation
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string SecretKey;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration) : base()
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            SecretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task< bool> IsUniqueUser(string UserId)
        {
            var user =await _unitOfWork.User.Get(l => l.Id == UserId);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponsDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _unitOfWork.User.Get(l => l.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (user == null || isValid == false)
            {
                return new LoginResponsDTO
                {
                    Token = "",
                    User = null
                };
            }
            //if user was found generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);

            Claim[] claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty), // Ensure roles.FirstOrDefault() is not null
                new Claim("username", user.UserName) // This line is correct and does not need changes
            };
            var tokenHander = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var tokenDescroptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHander.CreateToken(tokenDescroptor);
            LoginResponsDTO loginResponsDTO = new LoginResponsDTO
            {
                Token = tokenHander.WriteToken(token),
                User = _mapper.Map<UserDTO>(user)
            };
            return loginResponsDTO;

        }

        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerationRequestDTO.UserName,
                Email = registerationRequestDTO.UserName,
                NormalizedEmail = registerationRequestDTO.UserName.ToUpper(),
                FullName = registerationRequestDTO.FullName,
                PasswordHash=registerationRequestDTO.Password

            };
            try
            {
                var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    }
                    if (!await _roleManager.RoleExistsAsync("Customer"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Customer"));
                    }

                    await _userManager.AddToRoleAsync(user, "Customer");
                    var userToReturn = await _unitOfWork.User.Get(a => a.UserName == registerationRequestDTO.UserName);
                    return _mapper.Map<UserDTO>(userToReturn);

                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"User creation failed: {errors}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Registration failed: " + ex.Message);
                throw;
            }

            
        }

        public async Task<UserDTO> GetUser(string UserId)
        {
            var user = await _unitOfWork.User.Get(u => u.Id == UserId);
            return _mapper.Map<UserDTO>(user);
        }
    }
}
