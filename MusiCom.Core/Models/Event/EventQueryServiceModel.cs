using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.Event
{
    /// <summary>
    /// Holds information about the Events which will be presented after a given had been applied
    /// </summary>
    public class EventQueryServiceModel
    {
        public int TotalEventsCount { get; set; }

        public IEnumerable<EventAllViewModel> Events { get; set; } = new List<EventAllViewModel>();
    }
}
