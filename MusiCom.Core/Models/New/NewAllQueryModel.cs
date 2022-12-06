using System.ComponentModel.DataAnnotations;

namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Contains Data about Possible Criterias
    /// </summary>
    public class NewAllQueryModel
    {
        public const int NewPerPage = 4;

        public string? Genre { get; set; }

        public string? Tag { get; set; }

        [Display(Name = "Search by text")]
        public string? SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalNewsCount { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<NewAllNewViewModel> News { get; set; } = new List<NewAllNewViewModel>();
    }
}
