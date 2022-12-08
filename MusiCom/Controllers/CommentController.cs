using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Constants;
using MusiCom.Core.Contracts;
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

        /// <summary>
        /// Creates Comment
        /// </summary>
        /// <param name="model">Model passed from the View</param>
        /// <param name="Id">New's Id</param>
        /// <returns>Redirects to Action Details from NewController</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(NewDetailsViewModel model, Guid Id)
        {
            var editor = await userManager.GetUserAsync(User);

            try
            {
                await commentService.CreateCommentAsync(model.CurrentComment, Id, editor.Id);
            }
            catch (Exception)
            {
                TempData[MessageConstant.WarningMessage] = "An Error occured";
                return RedirectToAction("Details", "New", new { id = Id });
            }

            return RedirectToAction("Details", "New", new { id = Id });
        }

        /// <summary>
        /// Adds a Like to the Comment
        /// </summary>
        /// <param name="cId">Comment's Id</param>
        /// <param name="mId">New's Id</param>
        /// <returns>Redirects to Details Action in New Controller</returns>
        [HttpGet]
        public async Task<IActionResult> AddLike(Guid cId, Guid nId)
        {
            var comment = await commentService.GetCommentByIdAsync(cId);

            if (comment == null)
            {
                TempData[MessageConstant.WarningMessage] = "An Error occured";
                return RedirectToAction("Details", "New", new { id = nId });
            }

            try
            {
                await commentService.AddLikeToCommentAsync(comment);
            }
            catch (Exception)
            {
                TempData[MessageConstant.WarningMessage] = "An Error occured";
                return RedirectToAction("Details", "New", new { id = nId });
            }

            return RedirectToAction("Details", "New", new { id = nId });
        }

        /// <summary>
        /// Adds a Dislike to the Comment
        /// </summary>
        /// <param name="cId">Comment's Id</param>
        /// <param name="mId">New's Id</param>
        /// <returns>Redirects to Details Action in New Controller</returns>
        [HttpGet]
        public async Task<IActionResult> AddDislike(Guid cId, Guid nId)
        {
            var comment = await commentService.GetCommentByIdAsync(cId);

            if (comment == null)
            {
                TempData[MessageConstant.WarningMessage] = "An Error occured";
                return RedirectToAction("Details", "New", new { id = nId });
            }

            try
            {
                await commentService.AddDislikeToCommentAsync(comment);
            }
            catch (Exception)
            {
                TempData[MessageConstant.WarningMessage] = "An Error occured";
                return RedirectToAction("Details", "New", new { id = nId });
            }

            return RedirectToAction("Details", "New", new { id = nId });
        }
    }
}
