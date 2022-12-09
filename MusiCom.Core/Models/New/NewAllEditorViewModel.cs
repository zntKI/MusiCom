using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Contains data for Artist for NewAllViewModel
    /// </summary>
    public class NewAllEditorViewModel
    {
        public Guid Id { get; set; }

        public string EditorName { get; set; } = null!;
    }
}
