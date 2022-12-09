using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MusiCom.Core.Constants;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Event;
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

            List<string> list = new List<string>()
            {
                "Title", "Image", "Date", "Id", "Genre", "EventPosts", "Description", "ArtistName"
            };
            foreach (var field in list)
            {
                ModelState.Remove(field);
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Event", new { id = Id });
            }
            
            try
            {
                await postService.CreatePostAsync(model.CurrentPost, Id, user.Id, image);
            }
            catch (Exception e)
            {
                if (e.Message == "Not an image")
                {
                    TempData[MessageConstant.ErrorMessage] = "Please insert an image";
                }
                else if (e.Message == "Not the right image format")
                {
                    TempData[MessageConstant.ErrorMessage] = "Please insert an image with one of the formats shown";
                }
                else if (e.Message == "Image else")
                {
                    TempData[MessageConstant.WarningMessage] = "An Error occured";
                }
                return RedirectToAction("Details", "Event", new { id = Id });
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
                var post = await postService.GetPostByIdAsync(pId);
                if (post == null)
                {
                    TempData[MessageConstant.ErrorMessage] = "Not found";
                    return RedirectToAction("Details", "Event", new { id = mId });
                }
                await postService.AddLikeToPostAsync(post);
            }
            catch (Exception)
            {
                TempData[MessageConstant.WarningMessage] = "An Error occured";
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
                var post = await postService.GetPostByIdAsync(pId);
                if (post == null)
                {
                    TempData[MessageConstant.ErrorMessage] = "Not found";
                    return RedirectToAction("Details", "Event", new { id = mId });
                }
                await postService.AddDislikeToPostAsync(post);
            }
            catch (Exception)
            {
                TempData[MessageConstant.WarningMessage] = "An Error occured";
            }

            return RedirectToAction("Details", "New", new { id = mId });
        }
    }
}
