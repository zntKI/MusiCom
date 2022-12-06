using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Admin.User;
using MusiCom.Infrastructure.Data.Entities;
using System.Security.Claims;

namespace MusiCom.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(IUserService _userService, UserManager<ApplicationUser> _userManager)
        {
            userService = _userService;
            userManager = _userManager;
        }

        /// <summary>
        /// Gets All Users corresponding to a given criteria
        /// </summary>
        /// <param name="query">Criteria</param>
        /// <returns>View with the Users</returns>
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] AllUsersQueryModel query)
        {
            var queryResult = await userService.All(
                query.Type,
                query.SearchTerm,
                query.CurrentPage,
                AllUsersQueryModel.UsersPerPage);

            query.TotalUsersCount = queryResult.TotalUsersCount;
            query.Users = queryResult.Users;

            var types = new List<string>()
            {
                "UserOnly",
                "Editor",
                "Artist",
                "Editor and Artist"
            };
            query.Types = types;

            return View(query);
        }

        /// <summary>
        /// Renders view to which a UserAddViewModel is passed
        /// </summary>
        /// <param name="Id">Id of the User to be promoted</param>
        /// <returns>A View</returns>
        [HttpGet]
        public IActionResult MakeEditor(Guid Id)
        {
            //if (Id == Guid.Empty)
            //{
            //    throw new InvalidOperationException();
            //}

            var model = new EditorAddViewModel()
            {
                UserId = Id
            };

            return View(model);
        }

        /// <summary>
        /// Validates the model passed by the view and calls a method from the Service to Create the Editor
        /// </summary>
        /// <param name="model">Model passed by the View</param>
        /// <param name="Id">Id of the User</param>
        /// <returns>Redirects to Action All</returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost]
        public async Task<IActionResult> MakeEditor(EditorAddViewModel model, Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByIdAsync(Id.ToString());

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            if (await userManager.IsInRoleAsync(user, "Editor"))
            {
                throw new InvalidOperationException();
            }

            try
            {
                await userManager.AddToRoleAsync(user, "Editor");
                await userService.CreateEditorAsync(model, user);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("All", "User");
        }

        /// <summary>
        /// Checks if such User exists and if He is in the Editor Role. Then it calls a method from the Service to Remove Editor
        /// </summary>
        /// <param name="Id">Id of the User</param>
        /// <returns>Redirects to Action All</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IActionResult> RemoveEditor(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            if (!(await userManager.IsInRoleAsync(user, "Editor")))
            {
                throw new InvalidOperationException();
            }

            try
            {
                await userManager.RemoveFromRoleAsync(user, "Editor");
                await userService.RemoveEditorAsync(user);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("All", "User");
        }

        /// <summary>
        /// Validates the model passed by the view and calls a method from the Service to Create the Artist
        /// </summary>
        /// <param name="Id">Id of the User</param>
        /// <returns>Redirects to Action All</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IActionResult> MakeArtist(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            if (await userManager.IsInRoleAsync(user, "Artist"))
            {
                throw new InvalidOperationException();
            }

            try
            {
                await userManager.AddToRoleAsync(user, "Artist");
                await userService.CreateArtistAsync(user);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("All", "User");
        }

        /// <summary>
        /// Checks if such User exists and if He is in the Artist Role. Then it calls a method from the Service to Remove Artist
        /// </summary>
        /// <param name="Id">Id of the User</param>
        /// <returns>Redirects to Action All</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IActionResult> RemoveArtist(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            if (!(await userManager.IsInRoleAsync(user, "Artist")))
            {
                throw new InvalidOperationException();
            }

            try
            {
                await userManager.RemoveFromRoleAsync(user, "Artist");
                await userService.RemoveArtistAsync(user);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("All", "User");
        }

        /// <summary>
        /// Finds the User by the given Id and then calls a method from the Service to Mark the User as Deleted
        /// </summary>
        /// <param name="Id">Id of the User</param>
        /// <returns>Redirects to Action All</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());

            if (user.Id == Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!))
            {
                throw new InvalidOperationException();
            }

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            try
            {
                await userService.DeleteUserAsync(user);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("All", "User");
        }

        /// <summary>
        /// Finds the User by the given Id and then calls a method from the Service to Bring User Back and Mark it as NotDeleted
        /// </summary>
        /// <param name="Id">Id of the User</param>
        /// <returns>Redirects to Action All</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IActionResult> BringBackUser(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());

            if (user == null)
            {
                throw new InvalidOperationException();
            }

            try
            {
                await userService.BringBackUserAsync(user);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("All", "User");
        }
    }
}
