using System.Web.Mvc;

namespace AsbaBank.Presentation.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Index", "Client");
        }
    }
}
