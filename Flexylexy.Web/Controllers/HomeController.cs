using System.Web.Mvc;
using Flexylexy.Web.Models;

namespace Flexylexy.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public JsonResult GetPlayers()
        {
            return Json(Roster.ConnectedPlayers, JsonRequestBehavior.AllowGet);
        }
    }
}