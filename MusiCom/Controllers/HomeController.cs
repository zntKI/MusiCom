using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Models;
using System.Diagnostics;

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
        /// Presents the last three News
        /// </summary>
        /// <returns>A View</returns>
        public IActionResult Index()
        {
            var news = newService.GetLastThreeNewsAsync();

            return View(news);
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