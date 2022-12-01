using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusiCom.Infrastructure.Data.Entities
{
    public class Artist
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime DateOfCreation { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
