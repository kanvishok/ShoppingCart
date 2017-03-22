using System.Threading.Tasks;
using System.Web.Mvc;

namespace ShoppingCart.Api.Controllers
{
    public class HomeController : Controller
    {
        public  ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
