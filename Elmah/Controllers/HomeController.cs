using System;
using System.Web.Mvc;

namespace WebApplication13.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Convert.ToInt32("aa");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}