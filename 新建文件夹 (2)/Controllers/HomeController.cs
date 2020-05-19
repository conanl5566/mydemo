using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace abpexceptionless.Controllers
{
    //https://github.com/liangshiw/LogDashboard
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Hello, this is the index!");
            _logger.LogError(exception: new Exception("test"), message: "");
            var client = new HttpClient();
            await client.GetStringAsync("https://www.cnblogs.com/");

            return View();
        }
    }
}