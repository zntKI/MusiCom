using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.Tag
{
    /// <summary>
    /// Contains Tag data for showing all Tags
    /// </summary>
    public class TagAllViewModel : TagViewModel
    {
        public Guid Id { get; init; }

        public DateTime DateOfCreation { get; set; }
    }
}
