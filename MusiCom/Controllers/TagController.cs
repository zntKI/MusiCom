﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Constants;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Tag;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Controllers
{
    /// <summary>
    /// Contains functionalities regarding the tags
    /// </summary>
    [Authorize(Roles = "Admin, Editor")]
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
        public async Task<IActionResult> All()
        {
            var tags = await tagService.GetAllTagsAsync();

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

            return RedirectToAction("All");
        }

        /// <summary>
        /// Finds the Tag with the given Id and then renders a View
        /// </summary>
        /// <param name="id">Id of the Tag which is about to be Edited</param>
        /// <returns>A View</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await tagService.GetTagByIdAsync(id);

            if (tag == null)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
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
        /// <param name="id">Id of the Tag which is Edited</param>
        /// <param name="model">Tag</param>
        /// <returns>Redirects to All Action</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, TagAllViewModel model)
        {
            var tag = await tagService.GetTagByIdAsync(id);

            if (tag == null)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await tagService.EditTagAsync(tag, model);

            return RedirectToAction("All");
        }

        /// <summary>
        /// Marks the given Tag as Deleted
        /// </summary>
        /// <param name="id">The Id of the given Tag</param>
        /// <returns>Redirects to All Action</returns>
        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var tag = await tagService.GetTagByIdAsync(id);

            if (tag == null)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            await tagService.DeleteTagAsync(tag);

            return RedirectToAction("All");
        }
    }
}
