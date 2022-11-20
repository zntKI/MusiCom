using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Genre;
using MusiCom.Core.Models.Tag;
using MusiCom.Core.Services;
using MusiCom.Infrastructure.Data.Entities.News;

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

        /// <summary>
        /// Renders a view which shows all tags
        /// </summary>
        /// <returns>A View</returns>
        [HttpGet]
        public IActionResult All()
        {
            var tags = tagService.GetAllTags();

            return View(tags);
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
        /// Adds a Tag
        /// </summary>
        /// <param name="model">receives the Tag model from the client</param>
        /// <returns>Redirects to Home page</returns>
        [HttpPost]
        public async Task<IActionResult> Add(TagViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await tagService.CreateTagAsync(model);

            return RedirectToAction("All", "Tag");
        }

        /// <summary>
        /// Finds the Tag with the given Id and then renders a View
        /// </summary>
        /// <param name="id">The Id of the Tag which is about to be Edited</param>
        /// <returns>A View</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Tag tag = await tagService.GetTagByIdAsync(id);

            //TODO:
            if (tag == null)
            {
                return BadRequest();
            }

            TagAllViewModel model = new TagAllViewModel()
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return View(model);
        }

        /// <summary>
        /// Edits the Tag
        /// </summary>
        /// <param name="id">The Id of the Tag which is Edited</param>
        /// <param name="model">The Tag</param>
        /// <returns>Redirects to All Action</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, TagAllViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await tagService.EditTagAsync(id, model);

            return RedirectToAction("All", "Tag");
        }

        /// <summary>
        /// Marks the given Tag as Deleted
        /// </summary>
        /// <param name="id">The Id of the given Tag</param>
        /// <returns>Redirects to All Action</returns>
        public async Task<IActionResult> Remove(Guid id)
        {
            //TODO:
            if ((await tagService.GetTagByIdAsync(id)) == null)
            {
                return BadRequest();
            }

            await tagService.DeleteTagAsync(id);

            return RedirectToAction("All", "Tag");
        }
    }
}
