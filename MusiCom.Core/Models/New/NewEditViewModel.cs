using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.New
{
    public class NewEditViewModel : NewAddViewModel
    {
        public Guid Id { get; set; }

        public new byte[]? TitleImage { get; set; }
    }
}
