using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.Event
{
    /// <summary>
    /// Contains data for Artist for EventAllViewModel
    /// </summary>
    public class EventAllArtistViewModel
    {
        public Guid Id { get; set; }

        public string ArtistName { get; set; } = null!;
    }
}
