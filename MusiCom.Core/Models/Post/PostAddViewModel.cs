using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.Post
{
    public class PostAddViewModel
    {
        public string Content { get; set; } = null!;

        public byte[]? Image { get; set; }
    }
}
