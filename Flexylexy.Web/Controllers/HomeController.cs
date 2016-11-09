using System.Linq;
using System.Web.Mvc;
using Flexylexy.Web.Models;

namespace Flexylexy.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoster _roster;

        //public HomeController() { }

        public HomeController(IRoster roster)
        {
            _roster = roster;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}