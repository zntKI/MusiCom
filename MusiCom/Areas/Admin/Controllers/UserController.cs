using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Admin.User;

namespace MusiCom.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        private readonly IUserService userService;

        public UserController(IUserService _userService)
        {
            userService = _userService;
        }

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
                "UsersOnly",
                "Editor",
                "Artist",
                "Editors and Artists"
            };
            query.Types = types;

            return View(query);
        }
    }
}
