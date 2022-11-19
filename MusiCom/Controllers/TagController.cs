using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Tag;

namespace MusiCom.Controllers
{
    /// <summary>
    /// Contains functionalities regarding the tags
    /// </summary>
    public class TagController : Controller
    {
        private readonly ITagService tagService;

        public TagController(ITagService _tagService)
        {
            tagService = _tagService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Renders the view for add functionality
        /// </summary>
        /// <returns>AddView</returns>
        [HttpGet]
        public IActionResult Add()
        {
            TagViewModel model = new TagViewModel();

            return View(model);
        }

        /// <summary>
        /// Adds a tag
        /// </summary>
        /// <param name="model">receives the tag model from the client</param>
        /// <returns>Redirects to Home page</returns>
        [HttpPost]
        public async Task<IActionResult> Add(TagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await tagService.CreateTagAsync(model);

            return RedirectToAction("Index", "Home");
        }
    }
}
