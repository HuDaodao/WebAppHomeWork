using System.Web.Mvc;

namespace WebAppHomeWork.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index","Employee");
        }
    }
}