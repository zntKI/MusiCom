using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.New;
using Microsoft.AspNetCore.Identity;
using MusiCom.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusiCom.Infrastructure.Data.Entities.News;
using MusiCom.Core.Models.Comment;
using MusiCom.Core.Contracts.Admin;
using Microsoft.AspNetCore.Authorization;
using MusiCom.Core.Models.Event;
using static MusiCom.Infrastructure.Data.DataConstraints;
using MusiCom.Core.Models.Tag;

namespace MusiCom.Controllers
{
    /// <summary>
    /// Contains functionalities regarding the news
    /// </summary>
    public class NewController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INewServices newService;
        private readonly IGenreService genreService;
        private readonly ITagService tagService;

        public NewController(INewServices _newService, IGenreService _genreService,
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

            var newGenres = await genreService.GetAllGenreNames();
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
            var tags = await tagService.GetAllTags();
            
            NewAddViewModel model = new NewAddViewModel()
            {
                Genres = genreService.GetAllGenres(),
                TagsAll = Selects(tags)
            };

            return View(model);
        }

        /// <summary>
        /// Checks the model passed by the View and then calls a method from the Service
        /// </summary>
        /// <param name="model">The Model passed by the View</param>
        /// <param name="TitlePhoto">The TitlePhotoFile passed by the View</param>
        /// <returns>Redirects to Action "Index" in HomeController</returns>
        [HttpPost]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> Add(NewAddViewModel model, IFormFile image)
        {
            var editor = await userManager.GetUserAsync(User);

            if (editor == null)
            {
                return BadRequest();
            }

            //TODO: Find a better solution
            ModelState.Remove(nameof(model.TitleImage));
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //TODO: Fix
            try
            {
                await newService.CreateNewAsync(editor.Id, model, image);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Presents the Details of a New
        /// </summary>
        /// <param name="Id">New Id</param>
        /// <returns>A View with the NewModel</returns>
        public async Task<IActionResult> Details(Guid Id)
        {
            New entity = await newService.GetNewByIdAsync(Id);

            //TODO: Fix
            if (entity == null)
            {
                return BadRequest();
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
        /// <exception cref="InvalidOperationException"></exception>
        [Authorize(Roles = "Artist, Admin")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var user = await userManager.GetUserAsync(User);
            var neww = await newService.GetNewByIdAsync(Id);

            if (user == null || neww == null)
            {
                throw new InvalidOperationException();
            }

            if (user.Id != neww.EditorId)
            {
                throw new InvalidOperationException();
            }

            try
            {
                await newService.DeleteNewAsync(neww);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("All");
        }

        /// <summary>
        /// Renders a View for Editing a New
        /// </summary>
        /// <param name="Id">Id of the New</param>
        /// <returns>View for Editing</returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpGet]
        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var user = await userManager.GetUserAsync(User);
            var neww = await newService.GetNewByIdAsync(Id);

            if (user == null || neww == null)
            {
                throw new InvalidOperationException();
            }

            if (user.Id != neww.EditorId)
            {
                throw new InvalidOperationException();
            }

            var model = new NewEditViewModel()
            {
                Id = neww.Id,
                Title = neww.Title,
                Content = neww.Content,
                TitleImage = neww.TitleImage,
                Genres = genreService.GetAllGenres(),
                TagsAll = Selects(await tagService.GetAllTags()),
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
        /// <exception cref="InvalidOperationException"></exception>
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
                throw new InvalidOperationException();
            }

            if (Id != model.Id)
            {
                throw new InvalidOperationException();
            }

            try
            {
                await newService.EditNewAsync(neww, model, image);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("All");
        }

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
