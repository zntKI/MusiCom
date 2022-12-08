namespace MusiCom.Core.Models.Admin.User
{
    /// <summary>
    /// Model containing the Sorted Users
    /// </summary>
    public class UserQueryServiceModel
    {
        public int TotalUsersCount { get; set; }

        public IEnumerable<UserServiceModel> Users { get; set; }
            = new List<UserServiceModel>();
    }
}
