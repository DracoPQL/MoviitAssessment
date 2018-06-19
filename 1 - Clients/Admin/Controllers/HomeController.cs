using System.Web.Mvc;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ingredients()
        {
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }
    }
}