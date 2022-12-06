using MusiCom.Core.Models.Genre;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.Event
{
    /// <summary>
    /// Contains information about the Event
    /// </summary>
    public class EventAllViewModel
    {
        public Guid Id { get; set; }

        public byte[] Image { get; set; } = null!;

        public string Title { get; set; } = null!;

        public DateTime Date { get; set; }

        public string Description { get; set; } = null!;

        public EventAllArtistViewModel Artistt { get; set; } = null!;
    }
}
