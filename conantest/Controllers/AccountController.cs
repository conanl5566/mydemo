using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using conantest.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace conantest.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            string permissionRedirectUrl = "/Home/index";
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl!= "/")
            {
                ViewBag.ReturnUrl = returnUrl;
            }
            else
            {
                ViewBag.ReturnUrl = permissionRedirectUrl;
            }



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




            if (HttpContext.User.Identity.IsAuthenticated)
            {
              var r=  HttpContext.User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.Name).Value;
            }


            return Redirect(ViewBag.ReturnUrl);
        }

    
        public IActionResult Logout()
        {
            return View();
        }



    }
}
