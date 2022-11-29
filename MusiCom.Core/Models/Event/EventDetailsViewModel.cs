using MusiCom.Core.Models.Post;
using MusiCom.Infrastructure.Data.Entities.Events;

namespace MusiCom.Core.Models.Event
{
    /// <summary>
    /// Contains data which will be displayed when Details Page is opened
    /// </summary>
    public class EventDetailsViewModel
    {
        public Guid Id { get; set; }

        public byte[] Image { get; set; } = null!;

        public string Title { get; set; } = null!;

        public DateTime Date { get; set; }

        public string Description { get; set; } = null!;

        public MusiCom.Infrastructure.Data.Entities.News.Genre Genre { get; set; } = null!;

        public string ArtistName { get; set; } = null!;

        public PostAddViewModel CurrentPost { get; set; }

        public IEnumerable<EventPost> EventPosts { get; set; } = new List<EventPost>();
    }
}
