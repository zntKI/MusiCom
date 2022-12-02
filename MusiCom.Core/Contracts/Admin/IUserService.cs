using MusiCom.Core.Models.Admin.User;

namespace MusiCom.Core.Contracts.Admin
{
    public interface IUserService
    {
        Task<UserQueryServiceModel> All(
            string? type = null,
            string? searchTerm = null,
            int currentPage = 1,
            int usersPerPage = 1);
    }
}
