using Microsoft.AspNetCore.Mvc;

namespace ECommerceLocal.Web.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
