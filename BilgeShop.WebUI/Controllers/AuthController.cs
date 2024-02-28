using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace BilgeShop.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("Kayit-Ol")]    
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Kayit-Ol")]
        public IActionResult Register(RegisterViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var userAddDto = new UserAddDto()
            {
                Email = formData.Email.Trim(),
                FirstName = formData.FirstName.Trim(),
                LastName = formData.LastName.Trim(),
                Password = formData.Password,
            };

            var result = _userService.AddUser(userAddDto);

            if (result.IsSucceed)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return View(formData);
            }


        }

        public async Task<IActionResult> Login(LoginViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            var userLoginDto = new UserLoginDto()
            {
                Email = formData.Email,
                Password = formData.Password,
            };

            var userInfo = _userService.LoginUser(userLoginDto);

            if(userInfo is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var claims = new List<Claim>();

            claims.Add(new Claim("id", userInfo.Id.ToString()));
            claims.Add(new Claim("email", userInfo.Email));
            claims.Add(new Claim("firstName", userInfo.FirstName));
            claims.Add(new Claim("lastName", userInfo.LastName));
            claims.Add(new Claim("userType", userInfo.UserType.ToString()));

            claims.Add(new Claim(ClaimTypes.Role, userInfo.UserType.ToString())); // çokomelli

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var autProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(48)),
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), autProperties);

            @TempData["SuccessMessage"] = "Oturum açma başarılı.";

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {


            await HttpContext.SignOutAsync();
            TempData["SuccessMessage"] = "Oturum sonlandırıldı.";
            return RedirectToAction("Index", "Home");
        }
    }
}
