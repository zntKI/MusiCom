using Microsoft.AspNetCore.Mvc;

namespace MusiCom.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        public IActionResult All()
        {
            return View();
        }
    }
}
