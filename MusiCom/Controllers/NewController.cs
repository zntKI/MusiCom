using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusiCom.Core.Constants;
using MusiCom.Core.Contracts;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Comment;
using MusiCom.Core.Models.New;
using MusiCom.Core.Models.Tag;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Controllers
{
    /// <summary>
    /// Contains functionalities regarding the news
    /// </summary>
    public class NewController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INewService newService;
        private readonly IGenreService genreService;
        private readonly ITagService tagService;

        public NewController(INewService _newService, IGenreService _genreService,
                             ITagService _tagService, UserManager<ApplicationUser> _userManager)
        {
            newService = _newService;
            genreService = _genreService;
            tagService = _tagService;
            userManager = _userManager;
        }

        /// <summary>
        /// Shows All News or News which correspond to a given criteria
        /// </summary>
        /// <param name="query">The sorting parameters passed by the Url</param>
        /// <returns>View with the applied sorting</returns>
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] NewAllQueryModel query)
        {
            var queryResult = await newService.GetAllNewsAsync(
                query.Genre,
                query.Tag,
                query.SearchTerm,
                query.CurrentPage,
                NewAllQueryModel.NewPerPage);

            query.TotalNewsCount = queryResult.TotakNewsCount;
            query.News = queryResult.News;

            var newGenres = await genreService.GetAllGenreNamesAsync();
            query.Genres = newGenres;
            var newTags = await tagService.GetAllTagNamesAsync();
            query.Tags = newTags;

            return View(query);
        }

        /// <summary>
        /// Creates a ViewModel for adding a New, setting to it all Genres and Tags
        /// </summary>
        /// <returns>A View with the Model</returns>
        [HttpGet]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> Add()
        {
            var tags = await tagService.GetAllTagsAsync();
            
            NewAddViewModel model = new NewAddViewModel()
            {
                Genres = await genreService.GetAllGenresAsync(),
                TagsAll = Selects(tags)
            };

            return View(model);
        }

        /// <summary>
        /// Checks the model passed by the View and then calls a method from the Service
        /// </summary>
        /// <param name="model">The Model passed by the View</param>
        /// <param name="image">The TitlePhotoFile passed by the View</param>
        /// <returns>Redirects to Action "Index" in HomeController</returns>
        [HttpPost]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> Add(NewAddViewModel model, IFormFile image)
        {
            var editor = await userManager.GetUserAsync(User);

            ModelState.Remove(nameof(model.TitleImage));
            if (!ModelState.IsValid || image == null)
            {
                model.Genres = await genreService.GetAllGenresAsync();
                model.TagsAll = Selects(await tagService.GetAllTagsAsync());
                return View(model);
            }

            try
            {
                await newService.CreateNewAsync(editor.Id, model, image);
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
                model.Genres = await genreService.GetAllGenresAsync();
                model.TagsAll = Selects(await tagService.GetAllTagsAsync());
                return View(model);
            }

            return RedirectToAction("All");
        }

        /// <summary>
        /// Presents the Details of a New
        /// </summary>
        /// <param name="Id">New Id</param>
        /// <returns>A View with the NewModel</returns>
        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var entity = await newService.GetNewByIdAsync(Id);

            if (entity == null)
            {
                TempData[MessageConstant.WarningMessage] = "Not found";
                return RedirectToAction("All");
            }

            NewDetailsViewModel model = new NewDetailsViewModel()
            {
                Id = entity.Id,
                Title = entity.Title,
                TitleImage = entity.TitleImage,
                Content = entity.Content,
                Tags = newService.GetAllTagsForNew(entity.Id),
                Genre = await genreService.GetGenreByIdAsync(entity.GenreId),
                Editor = await userManager.FindByIdAsync(entity.EditorId.ToString()),
                NewComments = newService.GetAllCommentsForNew(Id),
                CurrentComment = new CommentAddViewModel()
            };

            return View(model);
        }

        /// <summary>
        /// Deletes the New with the given Id
        /// </summary>
        /// <param name="Id">Id of the New</param>
        /// <returns>Redirects to Action All</returns>
        [HttpPost]
        [Authorize(Roles = "Editor, Admin")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var user = await userManager.GetUserAsync(User);
            var neww = await newService.GetNewByIdAsync(Id);

            if (neww == null)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            if (user.Id != neww.EditorId)
            {
                TempData[MessageConstant.ErrorMessage] = "Can't delete other Editors's Events";
                return RedirectToAction("All");
            }

            try
            {
                await newService.DeleteNewAsync(neww);
            }
            catch (Exception)
            {
                TempData[MessageConstant.WarningMessage] = "An Error occured";
            }

            return RedirectToAction("All");
        }

        /// <summary>
        /// Renders a View for Editing a New
        /// </summary>
        /// <param name="Id">Id of the New</param>
        /// <returns>View for Editing</returns>
        [HttpGet]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var user = await userManager.GetUserAsync(User);
            var neww = await newService.GetNewByIdAsync(Id);

            if (neww == null)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            if (user.Id != neww.EditorId)
            {
                TempData[MessageConstant.ErrorMessage] = "You are not allowed to edit other Editor's News";
                return RedirectToAction("All");
            }

            var model = new NewEditViewModel()
            {
                Id = neww.Id,
                Title = neww.Title,
                Content = neww.Content,
                TitleImage = neww.TitleImage,
                Genres = await genreService.GetAllGenresAsync(),
                TagsAll = Selects(await tagService.GetAllTagsAsync()),
                EditorId = neww.EditorId,
                GenreId = neww.GenreId,
            };

            return View(model);
        }

        /// <summary>
        /// Edits a New
        /// </summary>
        /// <param name="Id">Id of the New</param>
        /// <param name="model">Edited Data for the New</param>
        /// <param name="image">New Image if there is such</param>
        /// <returns>Redirects to Action All</returns>
        [HttpPost]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> Edit(Guid Id, NewEditViewModel model, IFormFile image)
        {
            ModelState.Remove("image");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var neww = await newService.GetNewByIdAsync(Id);

            if (neww == null)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            if (Id != model.Id)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            try
            {
                await newService.EditNewAsync(neww, model, image);
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
            }

            return RedirectToAction("All");
        }

        /// <summary>
        /// Selects tags in a List of SelectListItem
        /// </summary>
        /// <param name="tags">New's tags</param>
        public List<SelectListItem> Selects(IEnumerable<TagNewAllViewModel> tags)
        {
            var selectList = new List<SelectListItem>();
            foreach (var tag in tags)
            {
                var item = new SelectListItem(tag.Name, tag.Id.ToString());
                selectList.Add(item);
            }

            return selectList;
        }
    }
}
