using System.ComponentModel;
using System.Web.Mvc;

namespace DynamicRoleBasedAccess.Controllers
{
    [Description("Home")]
    public class HomeController : Controller
    {
        [Description("Index")]
        public ActionResult Index()
        {
            return View();
        }

        [Description("About us")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Description("Contact us")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}