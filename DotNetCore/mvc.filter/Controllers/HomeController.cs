using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication11.Models;

namespace WebApplication11.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            // Convert.ToInt32("a");
            return View();
        }

        [TestFilter("002")]
        public IActionResult Privacy()
        {
            int a = 1;
            a += 1;
            var b = a;
            return View();
        }

        public IActionResult OnGet()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Content(HttpContext.User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.Name).Value);
            }
            return Content("未登录");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Content("退出");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> OnPost()
        {
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, "user.Email"),
                            new Claim("FullName", "user.FullName"),
                            new Claim(ClaimTypes.Role, "Administrator"),
                        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Content("登录完成");
        }

        [AllowAnonymous]
        public IActionResult login()
        {
            return Content("需要登录");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}