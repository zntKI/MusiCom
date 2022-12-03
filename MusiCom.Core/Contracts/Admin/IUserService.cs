using MusiCom.Core.Models.Admin.User;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Core.Contracts.Admin
{
    public interface IUserService
    {
        Task<UserQueryServiceModel> All(
            string? type = null,
            string? searchTerm = null,
            int currentPage = 1,
            int usersPerPage = 1);

        Task CreateEditorAsync(EditorAddViewModel model, ApplicationUser user);

        Task RemoveEditorAsync(ApplicationUser user);

        Task CreateArtistAsync(ApplicationUser user);

        Task RemoveArtistAsync(ApplicationUser user);

        Task DeleteUser(ApplicationUser user);
    }
}
