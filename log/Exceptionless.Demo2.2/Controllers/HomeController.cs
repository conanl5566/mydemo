using Exceptionless.Net.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Exceptionless.Net.Controllers
{
    public class HomeController : Controller
    {
        ////private ILog log;

        ////public HomeController(IHostingEnvironment hostingEnv)
        ////{
        ////    this.log = LogManager.GetLogger(Startup.repository.Name, typeof(HomeController));
        ////}

        private ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> _logger)
        {
            logger = _logger;
        }

        // GET api/val

        public IActionResult Index()
        {
            logger.LogError(new Exception("ExceptionDemo 的异常"), "qqqqqqqq");
            return View();
        }

        //public IActionResult Index()
        //{
        //    try
        //    {
        //        // Submit logs
        //        ExceptionlessClient.Default.SubmitLog("Logging made easy");

        //        // You can also specify the log source and log level.
        //        // We recommend specifying one of the following log levels: Trace, Debug, Info, Warn, Error
        //        ExceptionlessClient.Default.SubmitLog(typeof(Program).FullName, "This is so easy", "Info");
        //        ExceptionlessClient.Default.CreateLog(typeof(Program).FullName, "This is so easy", "Info").AddTags("Exceptionless").Submit();

        //        // Submit feature usages
        //        ExceptionlessClient.Default.SubmitFeatureUsage("MyFeature");
        //        ExceptionlessClient.Default.CreateFeatureUsage("MyFeature").AddTags("Exceptionless").Submit();

        //        // Submit a 404
        //        ExceptionlessClient.Default.SubmitNotFound("/somepage");
        //        ExceptionlessClient.Default.CreateNotFound("/somepage").AddTags("Exceptionless").Submit();

        //        // Submit a custom event type
        //        ExceptionlessClient.Default.SubmitEvent(new Event { Message = "Low Fuel", Type = "racecar", Source = "Fuel System" });

        //        throw new Exception("ExceptionDemo 的异常");
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToExceptionless().Submit();
        //    }
        //    return View();
        //}

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