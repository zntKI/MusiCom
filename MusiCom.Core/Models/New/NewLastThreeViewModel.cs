using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Contains data for showing lst Three News
    /// </summary>
    public class NewLastThreeViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public byte[] TitlePhoto { get; set; }
    }
}
