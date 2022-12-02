namespace MusiCom.Core.Models.Admin.User
{
    public class UserQueryServiceModel
    {
        public int TotalUsersCount { get; set; }

        public IEnumerable<UserServiceModel> Users { get; set; }
            = new List<UserServiceModel>();
    }
}
