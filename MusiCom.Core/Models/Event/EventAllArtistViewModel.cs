using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.Event
{
    public class EventAllArtistViewModel
    {
        public Guid Id { get; set; }

        public string ArtistName { get; set; } = null!;
    }
}
