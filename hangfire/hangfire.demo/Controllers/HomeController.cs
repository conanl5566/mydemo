using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hangfiretest.Models;
using Hangfire;

namespace hangfiretest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            RecurringJob.AddOrUpdate(() => Console.WriteLine(DateTime.Now.ToString()), "0 0/1 * * * ?", queue: "test");
            return View();
            return Redirect("/hangfire");
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
