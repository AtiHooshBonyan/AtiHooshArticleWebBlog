using Microsoft.AspNetCore.Mvc;

namespace Article.MVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
