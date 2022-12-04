using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Admin.User;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Core.Services.Admin
{
    /// <summary>
    /// Implements IUserService interface and contains the logic for the controllers
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IRepository _repo, UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }

        public async Task<UserQueryServiceModel> All(string? type = null, string? searchTerm = null, int currentPage = 1, int usersPerPage = 1)
        {
            var usersQuery = repo.AllReadonly<ApplicationUser>();

            if (!string.IsNullOrWhiteSpace(type))
            {
                if (type == "UserOnly")
                {
                    usersQuery = usersQuery
                        .Where(u => u.EditorId == null && u.ArtistId == null);
                }
                else if (type == "Editor")
                {
                    usersQuery = usersQuery
                        .Where(u => u.EditorId != null && u.ArtistId == null);
                }
                else if (type == "Artist")
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
                    ArtistId = e.ArtistId,
                    IsDeleted = e.IsDeleted
                })
                .ToListAsync();

            var totalUsers = await usersQuery.CountAsync();

            return new UserQueryServiceModel()
            {
                TotalUsersCount = totalUsers,
                Users = users
            };
        }

        /// <summary>
        /// Marks a User as NotDeleted
        /// </summary>
        /// <param name="user">User who is to be marked as NotDeleted</param>
        public async Task BringBackUserAsync(ApplicationUser user)
        {
            user.IsDeleted = false;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Creates Artist
        /// </summary>
        /// <param name="user">User who is to be assigned as an Artist</param>
        public async Task CreateArtistAsync(ApplicationUser user)
        {
            var artist = new Artist()
            {
                DateOfCreation = DateTime.Now,
                IsDeleted = false
            };

            await repo.AddAsync(artist);

            user.ArtistId = artist.Id;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Creates Editor
        /// </summary>
        /// <param name="model">Model with data to save passed by the controller</param>
        /// <param name="user">User who is to be assigned as an Editor</param>
        public async Task CreateEditorAsync(EditorAddViewModel model, ApplicationUser user)
        {
            var editor = new Editor()
            {
                HireDate = DateTime.Now,
                Salary = model.Salary,
                DateOfCreation = DateTime.Now,
                IsDeleted = false
            };

            await repo.AddAsync(editor);

            user.EditorId = editor.Id;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Marks a User as Deleted
        /// </summary>
        /// <param name="user">User who is to be marked as Deleted</param>
        public async Task DeleteUserAsync(ApplicationUser user)
        {
            user.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Marks the given Artist as Deleted
        /// </summary>
        /// <param name="user">The User who is to be unassigned from the Artist Role</param>
        public async Task RemoveArtistAsync(ApplicationUser user)
        {
            var artist = await repo.GetByIdAsync<Artist>(user.ArtistId!);

            user.ArtistId = null;

            artist.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Marks the given Editor as Deleted
        /// </summary>
        /// <param name="user">The User who is to be unassigned from the Editor Role</param>
        public async Task RemoveEditorAsync(ApplicationUser user)
        {
            var editor = await repo.GetByIdAsync<Editor>(user.EditorId!);

            user.EditorId = null;
            
            editor.IsDeleted = true;

            await repo.SaveChangesAsync();
        }
    }
}
