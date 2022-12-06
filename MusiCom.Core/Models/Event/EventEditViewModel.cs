using MusiCom.Core.Models.Genre;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusiCom.Infrastructure.Data.DataConstraints.EventC;

namespace MusiCom.Core.Models.Event
{
    /// <summary>
    /// Contains data for Editing an Event
    /// </summary>
    public class EventEditViewModel : EventAddViewModel
    {
        public Guid Id { get; set; }

        public new DateTime? Date { get; set; }

        public new byte[]? Image { get; set; }
    }
}
