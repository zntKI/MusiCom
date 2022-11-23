﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Comment;
using MusiCom.Core.Models.New;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICommentService commentService;

        public CommentController(UserManager<ApplicationUser> _userManager, ICommentService _commentService)
        {
            userManager = _userManager;
            commentService = _commentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Creates Comment
        /// </summary>
        /// <param name="model">ViewModel passed from the View</param>
        /// <param name="Id">New Id</param>
        /// <returns>Redirects to Action Details from NewController</returns>
        [HttpPost]
        public async Task<IActionResult> Create(NewDetailsViewModel model, Guid Id)
        {
            var editor = await userManager.GetUserAsync(User);

            if (editor == null)
            {
                return BadRequest();
            }

            await commentService.CreateCommentAsync(model.CurrentComment, Id, editor.Id);

            return RedirectToAction("Details", "New", new { id = Id });
        }
    }
}