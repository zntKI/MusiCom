using MusiCom.Core.Models.Admin.User;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Core.Contracts.Admin
{
    public interface IUserService
    {
        /// <summary>
        /// Sorts the users by the given criterias
        /// </summary>
        /// <param name="type">Type of User - (UserOnly, Editor and Artist, Editor or Artist)</param>
        /// <param name="searchTerm">What to search for</param>
        /// <param name="currentPage">The current page</param>
        /// <param name="usersPerPage">Nubmer of Users that could be displayed on a single page</param>
        /// <returns>Model containing Total Number of Users and Collection containing the Users</returns>
        Task<UserQueryServiceModel> AllAsync(
            string? type = null,
            string? searchTerm = null,
            int currentPage = 1,
            int usersPerPage = 1);

        /// <summary>
        /// Creates Editor
        /// </summary>
        /// <param name="model">Model with data to save passed by the controller</param>
        /// <param name="user">User who is to be assigned as an Editor</param>
        Task CreateEditorAsync(EditorAddViewModel model, ApplicationUser user);

        /// <summary>
        /// Marks the given Editor as Deleted
        /// </summary>
        /// <param name="user">The User who is to be unassigned from the Editor Role</param>
        Task RemoveEditorAsync(ApplicationUser user);

        /// <summary>
        /// Creates Artist
        /// </summary>
        /// <param name="user">User who is to be assigned as an Artist</param>
        Task CreateArtistAsync(ApplicationUser user);

        /// <summary>
        /// Marks the given Artist as Deleted
        /// </summary>
        /// <param name="user">The User who is to be unassigned from the Artist Role</param>
        Task RemoveArtistAsync(ApplicationUser user);

        /// <summary>
        /// Marks a User as Deleted
        /// </summary>
        /// <param name="user">User who is to be marked as Deleted</param>
        Task DeleteUserAsync(ApplicationUser user);

        /// <summary>
        /// Marks a User as NotDeleted
        /// </summary>
        /// <param name="user">User who is to be marked as NotDeleted</param>
        Task BringBackUserAsync(ApplicationUser user);
    }
}
