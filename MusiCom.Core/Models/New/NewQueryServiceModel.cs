using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Holds information about the News which will be presented after a given criteria had been applied
    /// </summary>
    public class NewQueryServiceModel
    {
        public int TotakNewsCount { get; set; }

        public IEnumerable<NewAllNewViewModel> News { get; set; }
            = new List<NewAllNewViewModel>();
    }
}
