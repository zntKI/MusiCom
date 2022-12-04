using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.New;
using MusiCom.Models;
using System.Diagnostics;
using static MusiCom.Areas.Admin.AdminConstants;

namespace MusiCom.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewServices newService;

        public HomeController(INewServices _newServices)
        {
            newService = _newServices;
        }

        /// <summary>
        /// Presents the all News
        /// </summary>
        /// <returns>A View</returns>
        public IActionResult Index()
        {
            if (User.IsInRole(AdminRoleName))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return RedirectToAction("All", "New");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}