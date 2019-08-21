using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {

        private readonly ICapPublisher _capBus;

        public HomeController(ICapPublisher capPublisher)
        {
            _capBus = capPublisher;
        }

        //////不使用事务
        ////[Route("~/without/transaction")]
        ////public IActionResult WithoutTransaction()
        ////{
        ////    _capBus.Publish("xxx.services.show.time", DateTime.Now);

        ////    return Ok();
        ////}

        public IActionResult Index()
        {
            _capBus.Publish("xxx.services.show.time", DateTime.Now);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
