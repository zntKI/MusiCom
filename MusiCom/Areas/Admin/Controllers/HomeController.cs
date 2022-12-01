using Microsoft.AspNetCore.Mvc;

namespace MusiCom.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
