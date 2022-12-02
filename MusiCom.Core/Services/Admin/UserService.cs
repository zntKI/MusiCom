using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Admin.User;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Core.Services.Admin
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;

        public UserService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task<UserQueryServiceModel> All(string? type = null, string? searchTerm = null, int currentPage = 1, int usersPerPage = 1)
        {
            var usersQuery = repo.AllReadonly<ApplicationUser>()
                .Where(u => u.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(type))
            {
                if (type == "UsersOnly")
                {
                    usersQuery = usersQuery
                        .Where(u => u.EditorId == null && u.ArtistId == null);
                }
                else if (type == "Editors")
                {
                    usersQuery = usersQuery
                        .Where(u => u.EditorId != null && u.ArtistId == null);
                }
                else if (type == "Artists")
                {
                    usersQuery = usersQuery
                        .Where(u => u.ArtistId != null && u.EditorId == null);
                }
                else if (type == "Editors and Artists")
                {
                    usersQuery = usersQuery
                        .Where(u => u.ArtistId != null && u.EditorId != null);
                }
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                usersQuery = usersQuery
                    .Where(u => EF.Functions.Like(u.UserName.ToLower(), searchTerm) ||
                          EF.Functions.Like(u.Email.ToLower(), searchTerm));
            }

            var users = await usersQuery
                .Skip((currentPage - 1) * usersPerPage)
                .Take(usersPerPage)
                .Select(e => new UserServiceModel
                {
                    Id = e.Id,
                    UserName = e.UserName,
                    Email = e.Email,
                    EditorId = e.EditorId,
                    ArtistId = e.ArtistId
                })
                .ToListAsync();

            var totalUsers = await usersQuery.CountAsync();

            return new UserQueryServiceModel()
            {
                TotalUsersCount = totalUsers,
                Users = users
            };
        }
    }
}
