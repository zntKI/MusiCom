using MusiCom.Infrastructure.Data.Entities;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Contains data for showing New details
    /// </summary>
    public class NewDetailsViewModel
    {
        public string Title { get; set; } = null!;

        public byte[] TitlePhoto { get; set; }

        public string Content { get; set; } = null!;

        public Infrastructure.Data.Entities.News.Genre Genre { get; set; }

        public ApplicationUser Editor { get; set; }

        public IEnumerable<Infrastructure.Data.Entities.News.Tag> Tags { get; set; } = new List<Infrastructure.Data.Entities.News.Tag>();

        public IEnumerable<NewComment> NewComments { get; set; } = new List<NewComment>();
    }
}
