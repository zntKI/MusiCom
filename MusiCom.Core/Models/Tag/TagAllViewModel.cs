using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.Tag
{
    public class TagAllViewModel : TagViewModel
    {
        public Guid Id { get; init; }

        public DateTime DateOfCreation { get; set; }
    }
}
