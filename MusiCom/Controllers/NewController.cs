using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.New;
using Microsoft.AspNetCore.Identity;
using MusiCom.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusiCom.Infrastructure.Data.Entities.News;

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

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Creates a ViewModel for adding a New, setting to it all Genres and Tags
        /// </summary>
        /// <returns>A View with the Model</returns>
        [HttpGet]
        public IActionResult Add()
        {
            var tags = tagService.GetAllTags();
            var selectList = new List<SelectListItem>();
            foreach (var tag in tags)
            {
                var item = new SelectListItem(tag.Name, tag.Id.ToString());
                selectList.Add(item);
            }

            NewAddViewModel model = new NewAddViewModel()
            {
                Genres = genreService.GetAllGenres(),
                TagsAll = selectList
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
        public async Task<IActionResult> Add(NewAddViewModel model, IFormFile TitlePhoto)
        {
            var editor = await userManager.GetUserAsync(User);

            if (editor == null)
            {
                return BadRequest();
            }

            //TODO: Find a better solution
            ModelState.Remove(nameof(model.TitlePhoto));
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await newService.CreateNewAsync(editor.Id, model, TitlePhoto);

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
                Title = entity.Title,
                TitlePhoto = entity.TitlePhoto,
                Content = entity.Content,
                Tags = newService.GetAllTagsForNew(entity.Id),
                Genre = await genreService.GetGenreByIdAsync(entity.GenreId),
                Editor = await userManager.FindByIdAsync(entity.EditorId.ToString()),
                NewComments = entity.NewComments
            };

            return View(model);
        }
    }
}
