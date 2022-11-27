using System.ComponentModel.DataAnnotations;

namespace MusiCom.Core.Models.Event
{
    /// <summary>
    /// Contains Data about Possible Criterias
    /// </summary>
    public class EventAllQueryModel
    {
        public const int EventsPerPage = 4;

        public string Genre { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalEventsCount { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public IEnumerable<EventAllViewModel> Events { get; set; } = new List<EventAllViewModel>();
    }
}
