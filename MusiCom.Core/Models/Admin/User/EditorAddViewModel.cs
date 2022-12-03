using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Core.Models.Admin.User
{
    /// <summary>
    /// ViewModel for adding User
    /// </summary>
    public class EditorAddViewModel
    {
        public Guid UserId { get; set; }

        [Required]
        [Range(typeof(decimal), "700.00", "99999.99", ErrorMessage = "The Salary must be between {1} and {2} lv.")]
        public decimal Salary { get; set; }
    }
}
