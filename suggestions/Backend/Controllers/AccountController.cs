using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Suggestions.Business.Abstract;
using Suggestions.Entities.Models;
using System.Security.Claims;

namespace Backend.Controllers
{
    public class AccountController : Controller
    {
        private IServiceManager _serviceManager;

        public AccountController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string Email, string Password)
        {
            if (!_serviceManager.UserService.CheckUser(Email, Password))
            {
                ModelState.AddModelError("Email", "Yanlıs E Mail veya Şifre Yeniden Deneyiniz");
                return View();
            }

            User? user = _serviceManager.UserService.GetUser(Email);
            //https://chsakell.com/2018/04/28/asp-net-core-identity-series-getting-started/
            if (user == null)
            {

            }
            else
            {
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Name + " " + user.SurName));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            }
            return RedirectToAction("Index", "Suggestions");
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn");
        }
    }
}
