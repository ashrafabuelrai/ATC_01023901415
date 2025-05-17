
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using EventBookingSystem.Web.Services.IServices;
using EventBookingSystem.Web.Models.DTOs.UserDTO;
using EventBookingSystem.Web.Models;
using EventBookingSystem.Application.Common.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using EventBookingSystem.Application.Contract;
using System.Globalization;

namespace EventBookingSystem.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITranslationService _translationService;
        string lang;
        public AuthController(IAuthService authService, ITranslationService translationService)
        {
            _authService = authService;
            _translationService = translationService;
            lang = CultureInfo.CurrentCulture.Name.StartsWith("ar") ? "ar" : "en";
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new LoginRequestDTO();
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            ApiResponse reslut;
            if (ModelState.IsValid)
            {
                reslut = await _authService.LoginAsync<ApiResponse>(obj);

                if (reslut != null && reslut.IsSuccess)
                {
                    LoginResponsDTO respons = JsonConvert.DeserializeObject<LoginResponsDTO>(Convert.ToString(reslut.Result));
                    HttpContext.Session.SetString(SD.SessionToken, respons.Token);

                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(respons.Token);

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                    identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                    var princepal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princepal);
                    HttpContext.Session.SetString("UserName", jwt.Claims.FirstOrDefault(u => u.Type == "unique_name")?.Value ?? "User");
                    HttpContext.Session.SetString("UserId", jwt.Claims.FirstOrDefault(u => u.Type == "nameid")?.Value ?? "User");
                    HttpContext.Session.SetString("UserRole", jwt.Claims.FirstOrDefault(u => u.Type == "role")?.Value ?? "User");
                    var currentCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.UICulture.Name ?? "en";
                    /* foreach (var claim in jwt.Claims)
                     {
                         Console.WriteLine($"{claim.Type}: {claim.Value}");
                     }
                     */
                    return RedirectToAction("Index", "Home");
                }
            }
            if (lang == "ar")
            {
                //var errorMessage = await _translationService.TranslateAsync(reslut.ErrorMessage.FirstOrDefault(), "en","ar");
               // ModelState.AddModelError("CustomError", errorMessage);
            }
            else
            {
                //ModelState.AddModelError("CustomError", reslut.ErrorMessage.FirstOrDefault());
            }

            return View(obj);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO obj)
        {
            if (ModelState.IsValid)
            {
                ApiResponse reslut = await _authService.RegisterAsync<ApiResponse>(obj);
                if (reslut != null && reslut.IsSuccess)
                {

                    return RedirectToAction("Login");
                }
            }
            return View(obj);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
