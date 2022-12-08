using System.ComponentModel.DataAnnotations;

namespace MusiCom.Core.Models.Admin.User
{
    /// <summary>
    /// Model holding the query parameters for Sorting Users
    /// </summary>
    public class AllUsersQueryModel
    {
        public const int UsersPerPage = 10;

        public string Type { get; set; } = null!;

        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; } = null!;

        public int CurrentPage { get; set; } = 1;

        public int TotalUsersCount { get; set; }

        public IEnumerable<string> Types { get; set; } = null!;

        public IEnumerable<UserServiceModel> Users { get; set; } = new List<UserServiceModel>();
    }
}
