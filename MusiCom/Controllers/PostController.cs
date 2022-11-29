using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Event;
using MusiCom.Core.Models.New;
using MusiCom.Core.Services;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Controllers
{
    public class PostController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;

        public PostController(UserManager<ApplicationUser> _userManager, IPostService _postService)
        {
            userManager = _userManager;
            postService = _postService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Creates a Post
        /// </summary>
        /// <param name="model">ViewModel passed from the View</param>
        /// <param name="Id">Event Id</param>
        /// <param name="image">Post Image</param>
        /// <returns>Redirects to Action Details from EventController</returns>
        [HttpPost]
        public async Task<IActionResult> Create(EventDetailsViewModel model, Guid Id, IFormFile image)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            //TODO: Fix
            try
            {
                await postService.CreatePostAsync(model.CurrentPost, Id, user.Id, image);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Details", "Event", new { id = Id });
        }

        /// <summary>
        /// Adds a Like to the Post
        /// </summary>
        /// <param name="pId">Post Id</param>
        /// <param name="mId">Event Id</param>
        /// <returns>Redirects to Details Action in Post Controller</returns>
        [HttpGet]
        public async Task<IActionResult> AddLike(Guid pId, Guid mId)
        {
            try
            {
                await postService.AddLikeToPost(pId);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Details", "Event", new { id = mId });
        }

        /// <summary>
        /// Adds a Dislike to the Post
        /// </summary>
        /// <param name="pId">Post Id</param>
        /// <param name="mId">Event Id</param>
        /// <returns>Redirects to Details Action in Post Controller</returns>
        [HttpGet]
        public async Task<IActionResult> AddDislike(Guid pId, Guid mId)
        {
            try
            {
                await postService.AddLikeToPost(pId);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Details", "New", new { id = mId });
        }
    }
}
